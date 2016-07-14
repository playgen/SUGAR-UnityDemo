using UnityEngine;

[RequireComponent(typeof(Controller))]
[RequireComponent(typeof(ResourceController))]
[RequireComponent(typeof(LoginController))]
[RequireComponent(typeof(SkillController))]
public class ScriptLocator : MonoBehaviour
{
	public static ResourceController ResourceController { get; private set; }
	public static Controller Controller { get; private set; }
	public static LoginController LoginController { get; private set; }
	public static SkillController SkillController { get; private set; }
    public static Config Config { get; set; }

	void Awake()
	{
		Controller = transform.GetComponent<Controller>();
		LoginController = transform.GetComponent<LoginController>();
		SkillController = transform.GetComponent<SkillController>();
		ResourceController = transform.GetComponent<ResourceController>();
	}

}
