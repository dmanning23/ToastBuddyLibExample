using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;
using Ouya.Console.Api;
using Microsoft.Xna.Framework;

namespace ToastBuddyLibExample.Ouya
{
	[Activity (Label = "ToastBuddyLibExample.Ouya", 
	           MainLauncher = true,
	           Icon = "@drawable/icon",
	           Theme = "@style/Theme.Splash",
                AlwaysRetainTaskState=true,
	           LaunchMode=Android.Content.PM.LaunchMode.SingleInstance,
	           ScreenOrientation = ScreenOrientation.Landscape,
	           ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | 
	                                  Android.Content.PM.ConfigChanges.KeyboardHidden | 
	                                  Android.Content.PM.ConfigChanges.Keyboard)]
	[IntentFilter(new[] { Intent.ActionMain }
	, Categories = new[] { Intent.CategoryLauncher, OuyaIntent.CategoryGame })]
	public class Activity1 : AndroidGameActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Create our OpenGL view, and display it
			Game1.Activity = this;
			var g = new Game1();
			SetContentView(g.Window);
			g.Run();
		}
	}
}


