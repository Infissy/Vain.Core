using System;
using System.Collections.Generic;
using Godot;
using Vain.Console;




namespace Vain.CLI.Log;

/// <summary>
///  OutputContainer: Class and Node that contains all the RichTextLabel containing the lines of the  <see cref="GameConsole"/>.
/// </summary>
public partial class OutputContainer : VBoxContainer
{

    [Export] Font Bold;
    [Export] Font Italic;
    [Export] Font BoldItalic;
    [Export] Font Normal;

    List<RichTextLabel> _items = new();
    int oldCount = 0;
    GameConsole console;
    ScrollContainer container;


    public override void _Ready()
    {
        base._Ready();

        console = GetOwner<GameConsole>();
        console.OnUpdate += OnBufferUpdate;
        container = GetParent<ScrollContainer>();

    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        //Keeps the scroll to the bottom
        if(oldCount < _items.Count ){
            container.ScrollVertical = (int)container.GetVScrollBar().MaxValue;
            oldCount = _items.Count;
        }
    }

    void OnBufferUpdate()
    {

        var output = console.BufferedMessages;

        for (int i = _items.Count; i < output.Count; i++)
        {
            var label = new RichTextLabel
            {
                FitContent = true,

                SizeFlagsHorizontal = SizeFlags.Expand | SizeFlags.Fill,

                BbcodeEnabled = true,

                Text = output[i]
            };

            if (Normal != null)
            {
                label.AddThemeFontOverride("normal_font",Normal) ;
                label.AddThemeFontOverride("bold_font",Bold) ;
                label.AddThemeFontOverride("italics_font",Italic) ;
                label.AddThemeFontOverride("bolditalics_font",BoldItalic) ;
            }

            _items.Add(label);
            AddChild(label);
        }

    }

}
