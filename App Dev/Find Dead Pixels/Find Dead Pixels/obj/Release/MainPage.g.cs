﻿

#pragma checksum "c:\users\vtquan\documents\visual studio 2012\Projects\Find Dead Pixels\Find Dead Pixels\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "DB7ADF34152343D41021B2AD11F24A13"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Find_Dead_Pixels
{
    partial class MainPage : global::Find_Dead_Pixels.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 54 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.Canvas_Tapped;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 48 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.redB_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 49 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.yellowB_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 50 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.greenB_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 51 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.blueB_Click;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 52 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.purpleB_Click;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 36 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoBack;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


