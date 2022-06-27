using System;
using System.Collections.Generic;
using Godot;
using Vain.Console;


/// <summary>
///  OutputContainer: Class and Node that contains all the RichTextLabel containing the lines of the  <see cref="GameConsole"/>.
/// </summary> 
public class OutputContainer : VBoxContainer
{

    [Export] Font Bold; 
    [Export] Font Italic; 
    [Export] Font BoldItalic; 
    [Export] Font Normal; 

    List<RichTextLabel> items = new List<RichTextLabel>();
    int oldCount = 0;
    GameConsole console;
    ScrollContainer container;


    /// <summary>
    /// <see cref="T:Godot.Node.Ready()"/>
    /// </summary> 
    public override void _Ready()
    {
        base._Ready();

        console = GetOwner<GameConsole>();
        console.OnUpdate += OnBufferUpdate;

        container = GetParent<ScrollContainer>();
    
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if(oldCount < items.Count ){
            container.ScrollVertical = (int)container.GetVScrollbar().MaxValue;
            oldCount = items.Count;
        }
    }

    void OnBufferUpdate()
    {
    
        var output = console.Buffer;

        for (int i = items.Count; i < output.Count; i++)
        {
            var label = new RichTextLabel();

            label.FitContentHeight = true;

            label.SizeFlagsHorizontal = (int) (SizeFlags.Expand | SizeFlags.Fill);
            
            label.BbcodeEnabled = true;

            label.BbcodeText = output[i];
            label.AddFontOverride("normal_font",Normal) ;
            label.AddFontOverride("bold_font",Bold) ;
            label.AddFontOverride("italics_font",Italic) ;
            label.AddFontOverride("bolditalics_font",BoldItalic) ;

            items.Add(label);
            AddChild(label);
        }          

     
        

        
        
    }

    



}