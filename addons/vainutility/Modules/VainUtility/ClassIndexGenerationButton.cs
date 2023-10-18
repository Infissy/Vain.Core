#if TOOLS

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Godot;
using Vain.Core;

using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace Vain.Plugins.VainUtility.GameIndex
{
    internal partial class ClassIndexGenerationButton : ButtonModule
    {

        readonly string _ClassIndexPath = ProjectConfig.LoadConfiguration(ProjectConfig.SingleSourceConfiguration.ClassIndex);
        
        //TODO: Currently work only for public code, add game specific code as well
        readonly string[] _SourceCodeFoldersPath = ProjectConfig.LoadConfiguration(ProjectConfig.MultiSourceConfiguration.SourceFolder);
        public override void _Ready()
        {
            base._Ready();
            ButtonName = "Generate Class Index";
            Pressed += GenerateClassIndex;
        }

        internal override string GetPluginName()
        {
            return "GameIndexUtility";
        }


        public void GenerateClassIndex()
        {

            var temporaryIndex = new Dictionary<string,CSharpScript>();

            
            
            
            var directoryStack = new Stack<string>();
            foreach(var path in _SourceCodeFoldersPath)
                directoryStack.Push(path);
            
            var currentDir = DirAccess.Open(directoryStack.Peek());
       


            while (directoryStack.Count > 0)
            {
                
                var folder = directoryStack.Pop();
                currentDir.ChangeDir(folder);
                var path = currentDir.GetCurrentDir();
                var dirs = currentDir.GetDirectories();


                foreach (var dir in dirs)
                    directoryStack.Push(path + "/" + dir);
                

                var files = currentDir.GetFiles();


                var scriptInDir = files.Select(f =>
                    new KeyValuePair<string,CSharpScript>(f.Replace(".cs",""),ResourceLoader.Load<CSharpScript>(path + "/" + f)
                    ))
                .Where( 
                    (kv) =>
                        {
                            
                            try
                            {
                                return kv.Value.New().Obj != null;
                            }
                            catch(MemberAccessException)
                            {
                                return false;
                            }
                            
                            
                        }
            );
                foreach (var script in scriptInDir)
                    temporaryIndex.Add(script.Key,script.Value);

                
            }

            var index = new IndexResource{IndexedEntities = new Godot.Collections.Dictionary<string, GodotObject>()};
            
            foreach (var customClass in temporaryIndex)
            {
                
                index.IndexedEntities[customClass.Key] = customClass.Value;

            }
            
            ResourceSaver.Save(index, _ClassIndexPath);
            index.EmitChanged();


        }




      
    }
}


#endif