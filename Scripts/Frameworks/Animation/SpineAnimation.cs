using Spine.Unity;

namespace Colored.Frameworks.Animation
{
    public class SpineAnimation : IAnimation
    {
        private SkeletonAnimation _skeletonAnimation;

        public SpineAnimation(SkeletonAnimation skeletonAnimation)
        {
            _skeletonAnimation = skeletonAnimation;
        }

        public void Play(string key, bool loop)
        {
            _skeletonAnimation.AnimationState.SetAnimation(0, key, loop);
        }

        public void Update(float elapsedTime)
        {
            
        }

        public void Stop()
        {
            _skeletonAnimation.AnimationState.ClearTrack(0);
        }
    }
}