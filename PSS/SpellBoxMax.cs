using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Forms.Design;
using System.Windows.Media;


[Designer(typeof(ControlDesigner))]
//[DesignerSerializer("System.Windows.Forms.Design.ControlCodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
class SpellBoxMax : ElementHost
{
    public SpellBoxMax()
    {
        box = new TextBox();
        //box.BorderThickness = new Thickness(1);
        box.BorderBrush =  Brushes.Black;
        base.Child = box;
        box.TextChanged += (s, e) => OnTextChanged(EventArgs.Empty);
        box.IsReadOnly = true;
        box.SpellCheck.IsEnabled = true;
        box.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        this.Size = new System.Drawing.Size(100, 20);
    }
    public override string Text
    {
        get { return box.Text; }
        set { box.Text = value; }
    }

    [DefaultValue(false)]
    public bool IsReadOnly
    {
        get { return box.IsReadOnly ; }
        set { box.IsReadOnly = value; }
    }
    [DefaultValue(32767)]
    public int MaxLength
    {
        get { return box.MaxLength; }
        set { box.MaxLength = value; }
    }

    [DefaultValue(false)]
    public bool Multiline
    {
        get { return box.AcceptsReturn; }
        set { box.AcceptsReturn = value; }
    }
    [DefaultValue(false)]
    public bool WordWrap
    {
        get { return box.TextWrapping != TextWrapping.NoWrap; }
        set { box.TextWrapping = value ? TextWrapping.Wrap : TextWrapping.NoWrap; }
    }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new System.Windows.UIElement Child
    {
        get { return base.Child; }
        set { /* Do nothing to solve a problem with the serializer !! */ }
    }
    private TextBox box;
}