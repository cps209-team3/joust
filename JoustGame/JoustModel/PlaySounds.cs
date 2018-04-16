using System.Threading.Tasks;
using System.IO;
using System.Threading;
// must reference Presentation core and WindowsBase

namespace JoustModel
{
    public class PlaySounds
    {
        //private SoundPlayer player;

        private string spawn;
        private string flap;
        private string walk;
        private string drop;
        private string collide;
        private string collect;
        private string select;

        public string path;

        private PlaySounds()
        {
            string newpath = Directory.GetCurrentDirectory();
            int indexPos = newpath.IndexOf("\\JoustGame");
            newpath = newpath.Substring(0, indexPos);
            newpath += "\\Sounds\\";
            path = newpath;

            spawn = path + "spawn.wav";
            flap = path + "flap.wav";
            walk = path + "walk.wav";
            drop = path + "drop.wav";
            collide = path + "collide.wav";
            collect = path + "collect.wav";
            select = path + "select.wav";

        }

        private static PlaySounds instance = new PlaySounds();
        public static PlaySounds Instance
        {
            get { return instance; }
        }

        public async void Play_Spawn()
        {
            //SoundPlayer player = new SoundPlayer(spawn);
            //await Task.Run(() => { player.Load(); player.Play(); });

            await Task.Run(() =>
            {
                var p1 = new System.Windows.Media.MediaPlayer();
                p1.Open(new System.Uri(spawn));
                p1.Play();
                Thread.Sleep(0);
            });
        }

        public async void Play_Flap()
        {
            //SoundPlayer player = new SoundPlayer(flap);
            //await Task.Run(() => { player.Load(); player.Play(); });

            await Task.Run(() =>
            {
                var p1 = new System.Windows.Media.MediaPlayer();
                p1.Open(new System.Uri(flap));
                p1.Play();
                Thread.Sleep(0);
            });
        }
        public async void Play_Walk()
        {
            //SoundPlayer player = new SoundPlayer(walk);
            //await Task.Run(() => { player.Load(); player.Play(); });

            await Task.Run(() =>
            {
                var p1 = new System.Windows.Media.MediaPlayer();
                p1.Open(new System.Uri(walk));
                p1.Play();
                Thread.Sleep(0);
            });
        }

        public async void Play_Drop()
        {
            //SoundPlayer player = new SoundPlayer(drop);
            //await Task.Run(() => { player.Load(); player.Play(); });

            await Task.Run(() =>
            {
                var p1 = new System.Windows.Media.MediaPlayer();
                p1.Open(new System.Uri(drop));
                p1.Play();
                Thread.Sleep(0);
            });
        }

        public async void Play_Collide()
        {
            //SoundPlayer player = new SoundPlayer(collide);
            //await Task.Run(() => { player.Load(); player.Play(); });

            await Task.Run(() =>
            {
                var p1 = new System.Windows.Media.MediaPlayer();
                p1.Open(new System.Uri(collide));
                p1.Play();
                Thread.Sleep(0);
            });
        }

        public async void Play_Collect()
        {
            //SoundPlayer player = new SoundPlayer(collect);
            //await Task.Run(() => { player.Load(); player.Play(); });

            await Task.Run(() =>
            {
                var p1 = new System.Windows.Media.MediaPlayer();
                p1.Open(new System.Uri(collect));
                p1.Play();
                Thread.Sleep(0);
            });
        }

        public async void Play_Select()
        {
            //SoundPlayer player = new SoundPlayer(select);
            //await Task.Run(() => { player.Load(); player.Play(); });

            await Task.Run(() =>
            {
                var p1 = new System.Windows.Media.MediaPlayer();
                p1.Open(new System.Uri(select));
                p1.Play();
                Thread.Sleep(0);
            });
        }
    }
}
