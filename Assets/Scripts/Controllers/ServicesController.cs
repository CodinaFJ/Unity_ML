using UnityEngine;

public class ServicesController : MonoBehaviour
{
	void Awake()
	{
		ServiceLocator.Instance.RegisterService(new LoggerService());
		ServiceLocator.Instance.RegisterService(new InputService());
		this.LogInfo("Services registered");
	}
}