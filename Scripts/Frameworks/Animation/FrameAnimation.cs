using UnityEngine;

namespace Colored.Frameworks.Animation
{
    public class FrameAnimation : IAnimation
    {
        private Sprite[]  _sprites;
        private SpriteRenderer  _newSpriteRenderer;
        private float _frameInterval = 0f; 
        private int _spriteFrame;
        private float _elapsedTime = 0f;
        private bool _loop;

        public FrameAnimation(Sprite[] sprites, SpriteRenderer spriteRenderer,float frameInterval)
        {
            _sprites = sprites;
            _newSpriteRenderer = spriteRenderer;
            _frameInterval = frameInterval;
        }
        
        public void Play(string key, bool loop)
        {
            _spriteFrame = 0;
            _elapsedTime = 0;
            _loop = loop;
        }
        public void Update(float elapsedTime)
        {
            _elapsedTime += elapsedTime;
            if (_elapsedTime > _frameInterval)
            {
                _elapsedTime = 0f;
                _spriteFrame = (_spriteFrame + 1) % _sprites.Length;
                _newSpriteRenderer.sprite = _sprites[_spriteFrame];
            }
        }

        public void Stop()
        {
            _spriteFrame = 0;
            _elapsedTime = 0;
        }
    }
}