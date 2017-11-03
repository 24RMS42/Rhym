using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using HockeyApp.iOS;
using UIKit;

namespace Rhym.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            var manager = BITHockeyManager.SharedHockeyManager;
            manager.Configure("b3bad20b1ee74335b7d277f7701fcabe");
            manager.StartManager();
            manager.Authenticator.AuthenticateInstallation();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
