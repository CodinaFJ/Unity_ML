using System;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
	[SerializeField][Range(0.1f, 1)] private float coyoteExtension;
	[SerializeField][Range(0.5f, 2f)] private float preJumpBuffer;
	[SerializeField] private LayerMask jumpableLayerMask;
	private bool onGround = true;
	private PlayerState pjState = PlayerState.IDLE;

	protected PolygonCollider2D     polygonCollider2D;
	protected SpriteRenderer        spriteRenderer;

	public Action	CollisionWithRewardAction;
	public Action	CollisionWithPunishAction;
	public Action	TriggerWithRewardAction;
	public Action	TriggerWithPunishAction;

	public bool OnGround 
	{
		get => onGround;
		set
		{
			if (value == onGround) return ;
			onGround = value;
			PjChangeOnGroundAction?.Invoke();
		}
	}

	public PlayerState PjState 
	{
		get => pjState;
		set 
		{
			if (value == pjState) return ;
			pjState = value;
			PjChangeStateAction?.Invoke();
		}
	}

	public Action PjChangeOnGroundAction;
	public Action PjChangeStateAction;

	protected virtual void Awake()
	{
		this.polygonCollider2D = this.GetComponent<PolygonCollider2D>();
		this.spriteRenderer = this.GetComponent<SpriteRenderer>();
	}

	private void Update() 
	{
		CheckOnGround();
	}

	protected virtual void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag(GameTag.REWARD))
			CollisionWithRewardAction?.Invoke();
		else if (other.gameObject.CompareTag(GameTag.PUNISH))
			CollisionWithPunishAction?.Invoke();
	}

	protected virtual void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag(GameTag.REWARD))
			TriggerWithRewardAction?.Invoke();
		else if (other.gameObject.CompareTag(GameTag.PUNISH))
			TriggerWithPunishAction?.Invoke();
	}
	
	private void CheckOnGround()
	{
		RaycastHit2D raycastHit2D1 = Physics2D.Raycast( (Vector2) transform.position + new Vector2(coyoteExtension, 0), Vector2.down, preJumpBuffer, jumpableLayerMask);
		Debug.DrawLine((Vector2) transform.position + new Vector2(coyoteExtension, 0), (Vector2) transform.position + new Vector2(coyoteExtension, 0) + Vector2.down * preJumpBuffer);
		RaycastHit2D raycastHit2D2 = Physics2D.Raycast( (Vector2) transform.position - new Vector2(coyoteExtension, 0), Vector2.down, preJumpBuffer, jumpableLayerMask);
		Debug.DrawLine((Vector2) transform.position - new Vector2(coyoteExtension, 0), (Vector2) transform.position - new Vector2(coyoteExtension, 0) + Vector2.down * preJumpBuffer);
		if (raycastHit2D1.collider != null || raycastHit2D2.collider != null)
			OnGround = true;
		else
			OnGround = false;
	}
}
