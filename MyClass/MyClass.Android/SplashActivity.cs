using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using System.Threading.Tasks;

namespace MyClass.Droid
{
    [Activity(Theme = "@style/AppTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            Log.Debug(TAG, "SplashActivity.OnCreate");
        }

        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { StartApp(); });
            startupWork.Start();
        }

        public override void OnBackPressed() { }

        async void StartApp()
        {
            await Task.Delay(500); // simulation d'un traitement qui doit être
                                   //avant le lancement de l'application (connection à un serveur, init DB ...)
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}