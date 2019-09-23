using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio; //WAVデータ
using Microsoft.Xna.Framework.Media; //MP3
using Microsoft.Xna.Framework.Content; //リソースへのアクセス
using System.Diagnostics;

namespace Scroll.GameSystem
{
    class Sound
    {
        #region フィールドとコンストラクタ
        private ContentManager contentManager;
        private Dictionary<string, Song> bgms;
        private Dictionary<string, SoundEffect> se;
        private Dictionary<string, SoundEffectInstance> seI;
        private Dictionary<string, SoundEffectInstance> seP;
        private string currentBGM;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="cm"></param>
        public Sound(ContentManager cm)
        {
            contentManager = cm;
            MediaPlayer.IsRepeating = true;

            bgms = new Dictionary<string, Song>();
            se = new Dictionary<string, SoundEffect>();
            seI = new Dictionary<string, SoundEffectInstance>();
            seP = new Dictionary<string, SoundEffectInstance>();
            currentBGM = null;
        }

        /// <summary>
        /// 解放
        /// </summary>
        public void Unload()
        {
            bgms.Clear();
            se.Clear();
            seI.Clear();
            seP.Clear();
        }
        #endregion フィールドとコンストラクタ

        /// <summary>
        /// Asset用のエラーメッセージ
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string ErrorMessage(string name)
        {
            return "再生する音声データのアセット名（" + name + "）がありません" +
                "アセット名の確認、Dictionalyに登録しているか確認してください";
        }

        #region BGM(MP3:MediaPlayer)関連

        /// <summary>
        /// BGMの読み込み
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filepath"></param>
        public void LoadBGM(string name, string filepath = "./")
        {
            //もう登録されてんねん
            if (bgms.ContainsKey(name))
                return;
            bgms.Add(name, contentManager.Load<Song>(filepath + name));
        }
        /// <summary>
        /// 停止中か
        /// </summary>
        /// <returns></returns>
        public bool IsStoppedBGM()
        {
            return (MediaPlayer.State == MediaState.Stopped);
        }

        /// <summary>
        ///再生中か 
        /// </summary>
        /// <returns></returns>
        public bool IsPlayingBGM()
        {
            return (MediaPlayer.State == MediaState.Playing);
        }

        /// <summary>
        /// 一時停止中か
        /// </summary>
        /// <returns></returns>
        public bool IsPauseBGM()
        {
            return (MediaPlayer.State == MediaState.Paused);
        }

        /// <summary>
        /// BGMの停止
        /// </summary>
        public void StopBGM()
        {
            MediaPlayer.Stop();
            currentBGM = null;
        }

        /// <summary>
        /// BGM再生
        /// </summary>
        public void PlayBGM(string name)
        {
            Debug.Assert(bgms.ContainsKey(name), ErrorMessage(name));
            //同じ曲か
            if (currentBGM == name)
                return;
            if (IsPlayingBGM())
                StopBGM();
            MediaPlayer.Volume = 1.0f;
            currentBGM = name;
            MediaPlayer.Play(bgms[currentBGM]);
        }
        public void PauseBGM()
        {
            if (IsPlayingBGM())
                MediaPlayer.Pause();
        }

        public void ChangeBGMLoopFlag(bool loopFlag)
        {
            MediaPlayer.IsRepeating = loopFlag;
        }
        #endregion BGM(MP3:MediaPlayer)関連

        #region WAV関連
        public void LoadSE(string name, string filepath = "./")
        {
            if (se.ContainsKey(name))
                return;
            se.Add(name, contentManager.Load<SoundEffect>(filepath + name));
        }

        public void PlaySE(string name)
        {
            Debug.Assert(se.ContainsKey(name), ErrorMessage(name));
            se[name].Play();
        }
        #endregion

        #region
        /// <summary>
        /// インスタンスの作成
        /// </summary>
        public void CreateSEInstance(string name)
        {
            if (seI.ContainsKey(name))
                return;
            Debug.Assert(se.ContainsKey(name),
                "先に" + name + "の読み込み処理を行ってください");
            seI.Add(name, se[name].CreateInstance());
        }

        public void PlayInstance(string name, int no, bool loopFlag = false)
        {
            Debug.Assert(seI.ContainsKey(name), ErrorMessage(name));
            if (seP.ContainsKey(name + no))
                return;
            var date = seI[name];
            date.IsLooped = loopFlag;
            date.Play();
            seP.Add(name + no, date);
        }

        public void StoppedSE(string name, int no)
        {
            if (seP.ContainsKey(name + no) == false)
                return;
            if (seP[name + no].State == SoundState.Playing)
                seP[name + no].Stop();
        }
        public void StoppedSE()
        {
            foreach (var se in seP)
            {
                if (se.Value.State == SoundState.Playing)
                    se.Value.Stop();
            }
        }
        public void RemoveSE(string name, int no)
        {
            if (seP.ContainsKey(name + no) == false)
                return;
            seP.Remove(name + no);
        }
        public void RemoveSE()
        {
            seP.Clear();
        }

        public void PouseSE(string name, int no)
        {
            if (seP.ContainsKey(name + no) == false)
                return;
            if (seP[name + no].State == SoundState.Playing)
                seP[name + no].Pause();
        }

        public void pauseSE()
        {
            foreach (var se in seP)
            {
                if (se.Value.State == SoundState.Playing)
                    se.Value.Pause();
            }
        }

        public void ResumeSE(string name, int no)
        {
            if (seP.ContainsKey(name + no) == false)
                return;
            if (seP[name + no].State == SoundState.Paused)
                seP[name + no].Resume();
        }

        public void ResumeSE()
        {
            foreach (var se in seP)
            {
                if (se.Value.State == SoundState.Paused)
                    se.Value.Resume();
            }
        }

        public bool IsPlayingSEInstance(string name, int np)
        {
            return seP[name + np].State == SoundState.Playing;
        }

        public bool IsStoppSEInstance(string name, int no)
        {
            return seP[name + no].State == SoundState.Stopped;
        }

        public bool IsPauseSEInstance(string name, int no)
        {
            return seP[name + no].State == SoundState.Paused;
        }
        #endregion
    }
}