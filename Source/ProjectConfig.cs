using System.Text.Json;
using Godot;

using System.Collections.Generic;
using System.Linq;
namespace Vain
{

    //FIXME: Minimum viable product, refactor it to be extendible 

    public class ProjectConfig
    {
        static List<ConfigFile> _ConfigurationFiles = new();


        static readonly string[] _ConfigurationPaths = new string[]{
            "res://vain.config.ini",
            "res://GameData/game.config.ini",
        };
        
        static readonly Dictionary<SingleSourceConfiguration,string[]> _SingleSourceConfigurations = new()
        {
            {SingleSourceConfiguration.ClassIndex, new string[]{"Indices", "ClassIndex"}},
            {SingleSourceConfiguration.EntityIndex, new string[]{"Indices", "EntityIndex"}},
            {SingleSourceConfiguration.BehaviourIndex, new string[]{"Indices", "BehaviourIndex"}},
            {SingleSourceConfiguration.ComponentIndex, new string[]{"Indices", "ComponentIndex"}},
            {SingleSourceConfiguration.LevelIndex, new string[]{"Indices", "LevelIndex"}},
            {SingleSourceConfiguration.CharacterFolder, new string[]{"Folders", "CharacterFolder"}},

        };

        static readonly Dictionary<MultiSourceConfiguration,string[]> _MultiSourceConfigurations = new()
        {
        
            {MultiSourceConfiguration.ComponentsFolder, new string[]{"Folders", "ComponentFolder"}},
            {MultiSourceConfiguration.LevelsFolder, new string[]{"Folders", "LevelFolder"}},
            {MultiSourceConfiguration.ScriptsFolder, new string[]{"Folders", "ScriptFolder"}},
            {MultiSourceConfiguration.SourceFolder, new string[]{"Folders", "SourceFolder"}},
           
        };

        static public string LoadConfiguration(SingleSourceConfiguration configuration){
            
            var configurationKeys = _SingleSourceConfigurations[configuration];
            var configurations = LoadConfiguration(configurationKeys[0],configurationKeys[1]); 
        
            if(configurations.Length == 0)
                throw new ConfigNotFoundException(configurationKeys[0],configurationKeys[1]);
            
            return configurations[0];  
            
        }

        static public string[] LoadConfiguration(MultiSourceConfiguration configuration){
            
            var configurationKeys = _MultiSourceConfigurations[configuration];
            var configurations = LoadConfiguration(configurationKeys[0],configurationKeys[1]); 
        
            if(configurations.Length == 0)
                throw new ConfigNotFoundException(configurationKeys[0],configurationKeys[1]);
            
            return configurations;
            
        }

        
       
        static public string[] LoadConfiguration(string section, string variable)
        {
            if(_ConfigurationFiles.Count == 0)
                LoadConfigurations();


            var result = new List<string>();

            foreach (var configFile in _ConfigurationFiles)
            {
                var configurationValue = configFile.GetValue(section,variable,"").Obj as string;

                if(!string.IsNullOrEmpty(configurationValue))
                    result.Add(configurationValue);
            }
            

            return result.ToArray();
           
        }


        static void LoadConfigurations()
        {
            foreach (var path in _ConfigurationPaths)
            {
                var configuration = new ConfigFile();

                //TODO: Handle Errors
                var result = configuration.Load(path);

                if(result == Error.Ok)
                    _ConfigurationFiles.Add(configuration);

            }
        }

        public enum SingleSourceConfiguration
        {
            ClassIndex,
            EntityIndex,
            ComponentIndex,
            BehaviourIndex,
            LevelIndex,
            CharacterFolder,
        }

        public enum MultiSourceConfiguration
        {
            ComponentsFolder,
            LevelsFolder,
            ScriptsFolder,
            SourceFolder,
      

        }
        
    }
}