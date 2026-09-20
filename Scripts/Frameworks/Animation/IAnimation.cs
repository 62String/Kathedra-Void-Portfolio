namespace Colored.Frameworks.Animation
{
    public interface IAnimation
    {
        public void Play(string key, bool loop);
        public void Update(float elapsedTime);
        public void Stop();
    }
}