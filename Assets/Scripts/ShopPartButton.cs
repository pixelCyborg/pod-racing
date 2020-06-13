using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPartButton : MonoBehaviour
{
    public CarComponent shopPart;
    public PartsDB.PartType type;
    private bool equipped;

    public void Initialize(GameObject componentObject, PartsDB.PartType type, bool equipped = false)
    {
        Button button = GetComponent<Button>();
        CarComponent component = componentObject.GetComponent<CarComponent>();
        shopPart = component;

        switch(type)
        {
            case PartsDB.PartType.Chassis:
                button.onClick.AddListener(() => {
                    Garage.instance.SetChassis(component as Chassis);
                    });
                break;
            case PartsDB.PartType.Engine:
                button.onClick.AddListener(() => {
                    Garage.instance.SetEngine(component as Engine);
                });
                break;
            case PartsDB.PartType.Booster:
                button.onClick.AddListener(() => {
                    Garage.instance.SetBooster(component as Booster);
                });
                break;
            case PartsDB.PartType.Wing:
                button.onClick.AddListener(() => {
                    Garage.instance.SetWing(component as Wing);
                });
                break;
        }

        GetComponentInChildren<Text>().text = componentObject.name;
    }

    public void ToggleEquip(bool _equipped)
    {
        equipped = _equipped;
        GetComponent<Button>().interactable = equipped;
    }
}
