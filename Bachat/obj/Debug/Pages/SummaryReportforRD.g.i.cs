﻿#pragma checksum "..\..\..\Pages\SummaryReportforRD.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B3E1F1886C61ADF5887C3F00E5FC0644"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Converters;
using FirstFloor.ModernUI.Windows.Navigation;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Bachat.Pages {
    
    
    /// <summary>
    /// SummaryReportforRD
    /// </summary>
    public partial class SummaryReportforRD : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 20 "..\..\..\Pages\SummaryReportforRD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbAccountNo;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Pages\SummaryReportforRD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dtpReportFromDate;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\Pages\SummaryReportforRD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dtpReportToDate;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\Pages\SummaryReportforRD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSubmit;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\Pages\SummaryReportforRD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal FirstFloor.ModernUI.Windows.Controls.ModernButton btnPrint;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\Pages\SummaryReportforRD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.FlowDocumentScrollViewer flwScrollViewSummaryReportRegister;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\Pages\SummaryReportforRD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Documents.FlowDocument flwSummaryReportRegister;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Bachat;component/pages/summaryreportforrd.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\SummaryReportforRD.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.cmbAccountNo = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 2:
            this.dtpReportFromDate = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 3:
            this.dtpReportToDate = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 4:
            this.btnSubmit = ((System.Windows.Controls.Button)(target));
            return;
            case 5:
            this.btnPrint = ((FirstFloor.ModernUI.Windows.Controls.ModernButton)(target));
            return;
            case 6:
            this.flwScrollViewSummaryReportRegister = ((System.Windows.Controls.FlowDocumentScrollViewer)(target));
            return;
            case 7:
            this.flwSummaryReportRegister = ((System.Windows.Documents.FlowDocument)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

