using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using EnvDTE80;
using EnvDTE;
using System.IO.Packaging;
using Microsoft.VisualStudio;
namespace App.Helpers
{
    public class ImageHelper
    {
        public void OptimizeImage(string filePath)
        {
            try
            {
            //    var DTE = (DTE2)Package.GetGlobalService(typeof(DTE));

            //    Command command = DTE.Commands.Item("ImageOptimizer.OptimizeLossless");

            //    if (command != null && command.IsAvailable)
            //    {
            //        DTE.Commands.Raise(command.Guid, command.ID, filePath, null);
            //    }
            }
            catch (Exception ex)
            {
                // Image Optimizer not installed
            }
        }
    }
}
