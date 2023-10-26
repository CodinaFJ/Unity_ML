using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
    [RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public abstract class EntityGraphics : MonoBehaviour
    {
        protected Animator          animator;
        protected SpriteRenderer    spriteRenderer;
        protected SpriteRenderer[]  childrenSpriteRenderers;

        protected const string HORIZONTAL_DIRECTION = "horizontal";
        protected const string VERTICAL_DIRECTION = "vertical";
        protected const string IS_MOVING = "isMoving";

        void Awake()
        {
            this.animator = this.GetComponent<Animator>();
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            this.childrenSpriteRenderers = this.GetComponentsInChildren<SpriteRenderer>();
        }

        public virtual void SetMovingAnimation(Vector2 movingDirection)
        {
            if(movingDirection != Vector2.zero)
            {
                SetAnimatorFloat(HORIZONTAL_DIRECTION, movingDirection.normalized.x);
                SetAnimatorFloat(VERTICAL_DIRECTION, movingDirection.normalized.y);
            }
            SetAnimatorBool(IS_MOVING, movingDirection != Vector2.zero);
        }

        public void SetAnimatorBool(string name, bool value)
        {
            animator.SetBool(name, value);
        }

        public void SetAnimatorTrigger(string name)
        {
            animator.SetTrigger(name);
        }

        public void SetAnimatorFloat(string name, float value)
        {
            animator.SetFloat(name, value);
        }

        // TODO: This method is confusing, should refactor this when doing combat and killing enemies
        public virtual void AnimationEventDestroy()
        {
            Destroy(this.gameObject);
        }

        public virtual void DisableAnimatedObject()
        {
            this.gameObject.SetActive(false);
        }

        public void SetSpriteRendererActive(bool setActive, bool recursive = false)
        {
            spriteRenderer.enabled = setActive;

            if(recursive)
            {
                foreach(SpriteRenderer childSpriteRenderer in childrenSpriteRenderers)
                {
                    childSpriteRenderer.enabled = setActive;
                }
            }
        }
    }
