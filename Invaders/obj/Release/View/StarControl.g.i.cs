﻿

#pragma checksum "C:\Users\Cauek\documents\visual studio 2012\Projects\Invaders\Invaders\View\StarControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A4022F0C9BC0577AD1831887B7FC669C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Invaders.View
{
    partial class StarControl : global::Windows.UI.Xaml.Controls.UserControl
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Media.Animation.Storyboard fadeInStoryboard; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Media.Animation.Storyboard fadeOutStoryboard; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Shapes.Polygon starPolygon; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///View/StarControl.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            fadeInStoryboard = (global::Windows.UI.Xaml.Media.Animation.Storyboard)this.FindName("fadeInStoryboard");
            fadeOutStoryboard = (global::Windows.UI.Xaml.Media.Animation.Storyboard)this.FindName("fadeOutStoryboard");
            starPolygon = (global::Windows.UI.Xaml.Shapes.Polygon)this.FindName("starPolygon");
        }
    }
}


