using UnityEngine;
using UnityEngine.Video;

namespace Canvas
{
    public class PlayVideoStep : Step
    {
        [SerializeField]
        VideoPlayer player;
        

        public override void Activate()
        {
            player.Play();
            Complete();
        }

        public override void Deactivate()
        {
            return;
        }

        public override void OnSlideExit()
        {
            if (player != null)
            {
                player.Stop();
            }
        }
    }

}