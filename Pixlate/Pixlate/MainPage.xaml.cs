using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Nokia.Graphics.Imaging;
using Pixlate.Resources;
using System.Windows.Media.Imaging;

namespace Pixlate
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            PhotoChooserTask pct = new PhotoChooserTask();
            pct.Completed += pct_Completed;
            pct.Show();
        }

        async void pct_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult != TaskResult.OK || e.ChosenPhoto == null)
                return;

            try
            {
                // Create a source to read the image from PhotoResult stream
                using (var source = new StreamImageSource(e.ChosenPhoto))
                {
                    using (var effect = new PixlateEffect(source))
                    {
                        //var target = new WriteableBitmap(1920, 1080);
                        var target = new WriteableBitmap((int)ResultImage.ActualWidth, (int)ResultImage.ActualHeight);
                        // Create a new renderer which outputs WriteableBitmaps
                        using (var renderer = new WriteableBitmapRenderer(effect, target))
                        {
                            // Render the image with the filter(s)
                            await renderer.RenderAsync();
                            // Set the output image to Image control as a source
                            ResultImage.Source = target;
                        }

                    }
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return;
            }
        }

    }
}