using UnityEngine;

public class ServicesController : MonoBehaviour
{
	void Awake()
	{
		ServiceLocator.Instance.RegisterService(new LoggerService());
		ServiceLocator.Instance.RegisterService(new InputService());
		ServiceLocator.Instance.RegisterService(new SpawnService());
		this.LogInfo("Services registered");
	}
}