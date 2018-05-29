using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoftDialogue {

    public abstract class DialogueContextObject {
        public DialogueContextObject(long id) {
            this.id = id;
        }
        public long id;
    }

    public class DialogueSprite : DialogueContextObject {
        public DialogueSprite(long id) : base(id) {
        }

        public void Free() {
            GameObject.Destroy(spriteRenderer.transform);
        }

        public void SetSource(string assetPath) {
            spriteRenderer.sprite = Resources.Load<Sprite>(assetPath);
        }

        public void SetSize(Vector2 size) {
            this.size = size;
            this.currentSize = size;
            if (snapPosition != SpriteSnapPosition.None) {
                spriteRenderer.transform.localScale = new Vector3(currentSize.x, currentSize.y, 1);
                spriteRenderer.transform.position = GetSnapedPosition(currentPosition, currentSize, snapPosition);
            }
        }
        public void SetPosition(Vector3 position) {
            this.position = position;
            this.currentPosition = position;
            snapPosition = SpriteSnapPosition.None;
            spriteRenderer.transform.position = this.position;
        }
        public void SnapToPosition(Vector3 position, SpriteSnapPosition snapPosition) {
            this.position = position;
            this.currentPosition = position;
            this.snapPosition = snapPosition;
            spriteRenderer.transform.position = GetSnapedPosition(currentPosition, currentSize, snapPosition);
        }

        public void Flip(bool flipX, bool flipY) {
            spriteRenderer.flipX = flipX;
            spriteRenderer.flipY = flipY;
        }

        public void SetVisible(bool visible) {
            spriteRenderer.enabled = visible;
        }

        private static Vector3 GetSnapedPosition(Vector3 position, Vector2 size, SpriteSnapPosition snapPosition) {
            switch (snapPosition) {
                case SpriteSnapPosition.TopLeft:
                    return position + new Vector3(size.x / 2, -size.y / 2);
                case SpriteSnapPosition.Top:
                    return position + new Vector3(0, -size.y / 2);
                case SpriteSnapPosition.TopRight:
                    return position + new Vector3(-size.x / 2, -size.y / 2);
                case SpriteSnapPosition.Left:
                    return position + new Vector3(size.x / 2, 0);
                case SpriteSnapPosition.Center:
                    return position + new Vector3(0, -0);
                case SpriteSnapPosition.Right:
                    return position + new Vector3(-size.x / 2, 0);
                case SpriteSnapPosition.BottomLeft:
                    return position + new Vector3(size.x / 2, size.y / 2);
                case SpriteSnapPosition.Bottom:
                    return position + new Vector3(0, size.y / 2);
                case SpriteSnapPosition.BottomRight:
                    return position + new Vector3(-size.x / 2, size.y / 2);
                default:
                    return position;
            }
        }

        public SpriteRenderer spriteRenderer;

        public SpriteSnapPosition snapPosition = SpriteSnapPosition.None;

        public Vector2 size;
        public Vector3 position;

        public void Scale(float scaleDuration, Vector2 targetSize) {
            this.size = currentSize;
            this.targetSize = targetSize;
            this.scaleDuration = scaleDuration;
            scaleProgression = 0;
            isScaling = true;
        }

        public void Move(float moveDuration, Vector3 targetPosition, SpriteSnapPosition targetSnapPosition) {
            this.position = currentPosition;
            this.targetPosition = targetPosition;
            this.moveDuration = moveDuration;
            this.targetSnapPosition = targetSnapPosition;
            moveProgression = 0;
            isMoving = true;
        }

        public bool Animate(float deltaTime) {
            if (isScaling)
                DoScaling(deltaTime);
            if (isMoving)
                DoMoving(deltaTime);
            if (isScaling || isMoving) {
                spriteRenderer.transform.localScale = new Vector3(currentSize.x, currentSize.y, 1);
                spriteRenderer.transform.position = currentPosition;
                return true;
            }
            return false;
        }

        private void DoScaling(float deltaTime) {
            scaleProgression += deltaTime / scaleDuration;
            if (scaleProgression >= 1)
                EndScaling();
            else {
                currentSize = size * (1 - scaleProgression) + targetSize * scaleProgression;
            }
        }

        private void DoMoving(float deltaTime) {
            moveProgression += deltaTime / moveDuration;
            if (moveProgression >= 1)
                EndMoving();
            else {
                currentPosition = GetSnapedPosition(position, currentSize, snapPosition) * (1 - moveProgression) +
                    GetSnapedPosition(targetPosition, currentSize, targetSnapPosition) * moveProgression;
            }
        }

        public void EndScaling() {
            SetSize(targetSize);
            isScaling = false;
        }

        public void EndMoving() {
            SnapToPosition(targetPosition, snapPosition);
            isMoving = false;
        }

        public bool isAnimating {
            get { return isScaling || isMoving; }
        }

        private bool isScaling;
        private Vector2 currentSize;
        private Vector2 targetSize;
        private float scaleDuration;
        private float scaleProgression;

        private bool isMoving;
        private Vector3 currentPosition;
        private Vector3 targetPosition;
        private SpriteSnapPosition targetSnapPosition;
        private float moveDuration;
        private float moveProgression;
    }

    public enum SpriteSnapPosition : short {
        None,
        TopLeft, Top, TopRight,
        Left, Center, Right,
        BottomLeft, Bottom, BottomRight
    }

    public class DialogSound : DialogueContextObject {

        public DialogSound(long id, AudioClip audioClip) : base(id) {
            this.audioClip = audioClip;
        }

        public AudioClip audioClip;

        public bool isPlaying {
            get { return true; }
        }
    }

}
