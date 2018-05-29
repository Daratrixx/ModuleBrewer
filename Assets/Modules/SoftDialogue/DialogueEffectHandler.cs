using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoftDialogue {

    public abstract class DialogueEffectHandler {
        public abstract void Handle(DialogueContext context);
    }

    #region EFFECT_TIME

    public class DialogueEffectWaitHandler : DialogueEffectHandler {
        public DialogueEffectWaitHandler(float duration) : base() {
            this.duration = duration;
        }

        public override void Handle(DialogueContext context) {
            context.Wait(duration);
        }

        private float duration;
    }

    #endregion

    #region EFFECT_CONDITION

    public class DialogueEffectSetConditionHandler : DialogueEffectHandler {
        public DialogueEffectSetConditionHandler(string condition) : base() {
            this.condition = condition;
        }

        public override void Handle(DialogueContext context) {
            context.SetCondition(condition);
        }

        private string condition;
    }
    public class DialogueEffectFreeConditionHandler : DialogueEffectHandler {
        public DialogueEffectFreeConditionHandler(string condition) : base() {
            this.condition = condition;
        }

        public override void Handle(DialogueContext context) {
            context.FreeCondition(condition);
        }

        private string condition;
    }

    #endregion

    #region EFFECT_OBJECTS

    public abstract class DialogueEffectContextObjectHandler : DialogueEffectHandler {
        public DialogueEffectContextObjectHandler(long objectId) : base() {
            this.objectId = objectId;
        }

        protected long objectId;
    }
    public abstract class DialogueEffectSetContextObjectHandler : DialogueEffectContextObjectHandler {
        public DialogueEffectSetContextObjectHandler(long objectId, System.Object o) : base(objectId) {
            this.o = o;
        }

        public override void Handle(DialogueContext context) {
            context.SetContextObject(objectId, o);
        }

        protected System.Object o;
    }
    public class DialogueEffectFreeContextObjectHandler : DialogueEffectContextObjectHandler {
        public DialogueEffectFreeContextObjectHandler(long objectId) : base(objectId) {
        }

        public override void Handle(DialogueContext context) {
            context.FreeContextObject(objectId);
        }
    }

    #endregion

    #region EFFECT_SPRITE

    public class DialogueEffectSetSpriteHandler : DialogueEffectSetContextObjectHandler {
        public DialogueEffectSetSpriteHandler(long objectId) : base(objectId, new DialogueSprite(objectId) { }) { }
    }
    public class DialogueEffectSetSpriteSourceHandler : DialogueEffectContextObjectHandler {
        public DialogueEffectSetSpriteSourceHandler(long objectId, string assetPath) : base(objectId) {
            this.assetPath = assetPath;
        }

        public override void Handle(DialogueContext context) {
            context.SetSpriteSource(objectId, assetPath);
        }

        protected string assetPath;
    }

    public class DialogueEffectSetSpriteVisibleHandler : DialogueEffectContextObjectHandler {
        public DialogueEffectSetSpriteVisibleHandler(long objectId, bool visible) : base(objectId) {
            this.visible = visible;
        }

        public override void Handle(DialogueContext context) {
            context.SetSpriteVisible(objectId, visible);
        }

        protected bool visible;
    }
    public class DialogueEffectSetSpriteSizeHandler : DialogueEffectContextObjectHandler {
        public DialogueEffectSetSpriteSizeHandler(long objectId, Vector2 size) : base(objectId) {
            this.size = size;
        }

        public override void Handle(DialogueContext context) {
            context.SetSpriteSize(objectId, size);
        }

        protected Vector2 size;
    }
    public class DialogueEffectSetSpritePoitionHandler : DialogueEffectContextObjectHandler {
        public DialogueEffectSetSpritePoitionHandler(long objectId, Vector3 position) : base(objectId) {
            this.position = position;
        }

        public override void Handle(DialogueContext context) {
            context.SetSpritePosition(objectId, position);
        }

        protected Vector3 position;
    }
    public class DialogueEffectSnapSpriteToPositionHandler : DialogueEffectContextObjectHandler {
        public DialogueEffectSnapSpriteToPositionHandler(long objectId, Vector3 position, SpriteSnapPosition snapPosition) : base(objectId) {
            this.position = position;
            this.snapPosition = snapPosition;
        }

        public override void Handle(DialogueContext context) {
            context.SnapSpriteToPosition(objectId, position, snapPosition);
        }

        protected Vector3 position;
        protected SpriteSnapPosition snapPosition;
    }
    public class DialogueEffectFlipSpriteHandler : DialogueEffectContextObjectHandler {
        public DialogueEffectFlipSpriteHandler(long objectId, bool flipX, bool flipY) : base(objectId) {
            this.flipX = flipX;
            this.flipY = flipY;
        }

        public override void Handle(DialogueContext context) {
            context.FlipSprite(objectId, flipX, flipY);
        }

        protected bool flipX;
        protected bool flipY;
    }

    public class DialogueEffectScaleSpriteHandler : DialogueEffectContextObjectHandler {
        public DialogueEffectScaleSpriteHandler(long objectId, float scaleDuration, Vector2 targetSize) : base(objectId) {
            this.scaleDuration = scaleDuration;
            this.targetSize = targetSize;
        }

        public override void Handle(DialogueContext context) {
            context.ScaleSprite(objectId, scaleDuration, targetSize);
        }

        protected float scaleDuration;
        protected Vector2 targetSize;
    }
    public class DialogueEffectMoveSpriteHandler : DialogueEffectContextObjectHandler {
        public DialogueEffectMoveSpriteHandler(long objectId, float moveDuration, Vector3 targetPosition, SpriteSnapPosition targetSnapPosition = SpriteSnapPosition.None) : base(objectId) {
            this.moveDuration = moveDuration;
            this.targetPosition = targetPosition;
            this.targetSnapPosition = targetSnapPosition;
        }

        public override void Handle(DialogueContext context) {
            context.MoveSprite(objectId, moveDuration, targetPosition, targetSnapPosition);
        }

        protected float moveDuration;
        protected Vector3 targetPosition;
        protected SpriteSnapPosition targetSnapPosition;
    }

    #endregion

    #region EFFECT_AUDIO

    public class DialogueEffectSetAudioClipHandler : DialogueEffectSetContextObjectHandler {
        public DialogueEffectSetAudioClipHandler(long objectId) : base(objectId, AudioClip.Create("N/A", 1, 1, 20050, false)) { // god damn unity update >.>

        }
    }
    public class DialogueEffectSetAudioClipSourceHandler : DialogueEffectContextObjectHandler {
        public DialogueEffectSetAudioClipSourceHandler(long objectId, string assetPath) : base(objectId) {
            this.assetPath = assetPath;
        }

        public override void Handle(DialogueContext context) {
            context.SetAudioSource(objectId, assetPath);
        }

        protected string assetPath;
    }

    public class DialogueEffectPlayAudioHandler : DialogueEffectContextObjectHandler {
        public DialogueEffectPlayAudioHandler(long objectId) : base(objectId) { }
        public override void Handle(DialogueContext context) {
            context.PlayAudio(objectId);
        }
    }
    public class DialogueEffectPlayMusicHandler : DialogueEffectContextObjectHandler {
        public DialogueEffectPlayMusicHandler(long objectId) : base(objectId) { }
        public override void Handle(DialogueContext context) {
            context.PlayMusic(objectId);
        }
    }

    public class DialogueEffectStopAudioHandler : DialogueEffectHandler {
        public override void Handle(DialogueContext context) {
            context.StopAudio();
        }
    }
    public class DialogueEffectStopMusicHandler : DialogueEffectHandler {
        public override void Handle(DialogueContext context) {
            context.StopMusic();
        }
    }

    #endregion

}
