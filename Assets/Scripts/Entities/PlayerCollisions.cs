using System;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
        protected PolygonCollider2D     polygonCollider2D;
        protected SpriteRenderer        spriteRenderer;

		public Action	CollisionWithRewardAction;
		public Action	CollisionWithPunishAction;
		public Action	TriggerWithRewardAction;
		public Action	TriggerWithPunishAction;

        protected virtual void Awake()
        {
            this.polygonCollider2D = this.GetComponent<PolygonCollider2D>();
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        protected virtual void OnCollisionEnter2D(Collision2D other)
        {
			if (other.gameObject.CompareTag("reward"))
				CollisionWithRewardAction?.Invoke();
			else if (other.gameObject.CompareTag("punish"))
				CollisionWithPunishAction?.Invoke();
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
			if (other.gameObject.CompareTag("reward"))
				TriggerWithRewardAction?.Invoke();
			else if (other.gameObject.CompareTag("punish"))
				TriggerWithPunishAction?.Invoke();
        }
}
