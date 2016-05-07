using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Bachat.Helpers
{
    public class TextBoxHelper
    {
        static List<String> isInistialised = new List<string>();

        public static string GetWatermark(DependencyObject obj)
        {
            return (string)obj.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarkProperty, value);
        }

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached("Watermark", typeof(string), typeof(TextBoxHelper), new UIPropertyMetadata(null, WatermarkChanged));



        public static bool GetShowWatermark(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowWatermarkProperty);
        }

        public static void SetShowWatermark(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowWatermarkProperty, value);
        }

        public static readonly DependencyProperty ShowWatermarkProperty =
            DependencyProperty.RegisterAttached("ShowWatermark", typeof(bool), typeof(TextBoxHelper), new UIPropertyMetadata(false));



        static void WatermarkChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var box = obj as TextBox;

            CheckShowWatermark(box);
            
            if (!isInistialised.Contains(box.Name))
            {
                box.TextChanged += new TextChangedEventHandler(box_TextChanged);
                box.Unloaded += new RoutedEventHandler(box_Unloaded);
                isInistialised.Add(box.Name);
            }
        }

        private static void CheckShowWatermark(TextBox box)
        {
            box.SetValue(TextBoxHelper.ShowWatermarkProperty, box.Text == string.Empty);
        }

        static void box_TextChanged(object sender, TextChangedEventArgs e)
        {
            var box = sender as TextBox;
            CheckShowWatermark(box);
        }

        static void box_Unloaded(object sender, RoutedEventArgs e)
        {
            var box = sender as TextBox;
            box.TextChanged -= new TextChangedEventHandler(box_TextChanged);
            isInistialised.Remove(box.Name);
        }

    }
}
