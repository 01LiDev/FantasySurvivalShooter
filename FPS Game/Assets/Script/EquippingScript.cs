using System.Collections;
using UnityEngine;
using UnityEngine.UI; 

public class EquippingScript : MonoBehaviour
{
    public GameObject SwordObj;
    public GameObject PistolObj;
    public GameObject RifleObj;
    public Animator PlayerAnim;


    public Text SwordUI;
    public Text PistolUI;
    public Text RifleUI;
    public bool PistolActive = false;
    public bool RifleActive = false;
    
    public bool SwordActive = false;
    private InventoryManager inventoryManager;

    
    void Start()
    {
        SwordObj.SetActive(true);
        PistolObj.SetActive(false);
        RifleObj.SetActive(false);
        // Update UI text elements
        SwordUI.enabled = true;
        PistolUI.enabled = false;
        RifleUI.enabled = false;
        if (PlayerAnim == null)
        {
            PlayerAnim = GetComponent<Animator>();
        }
        PlayerAnim.SetBool("MeleeActive", true);
        ChangeAnimationLayer(0);
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            Equip1();
            PlayerAnim.SetBool("MeleeActive", true);
        }

        if (Input.GetKeyDown("2") && inventoryManager.canPress2)
        {
            Equip2();
            PlayerAnim.SetBool("MeleeActive", false);
        }

        if (Input.GetKeyDown("3") && inventoryManager.canPress3)
        {
            Equip3();
            PlayerAnim.SetBool("MeleeActive", false);
        }
    }
   
    void Equip1()
    {
        SwordObj.SetActive(true);
        PistolObj.SetActive(false);
        RifleObj.SetActive(false);
        ChangeAnimationLayer(0);

        // Update UI text elements
        SwordUI.enabled = true;
        PistolUI.enabled = false;
        RifleUI.enabled = false;

    }

    void Equip2()
    {
        SwordObj.SetActive(false);
        PistolObj.SetActive(true);
        RifleObj.SetActive(false);
        ChangeAnimationLayer(1);

        // Update UI text elements
        SwordUI.enabled = false;
        PistolUI.enabled = true;
        RifleUI.enabled = false;

    }

    void Equip3()
    {
        SwordObj.SetActive(false);
        PistolObj.SetActive(false);
        RifleObj.SetActive(true);
        ChangeAnimationLayer(2);

        // Update UI text elements
        SwordUI.enabled = false;
        PistolUI.enabled = false;
        RifleUI.enabled = true;

    }

    void ChangeAnimationLayer(int activeLayer)
    {
        PlayerAnim.SetLayerWeight(0, activeLayer == 0 ? 1 : 0);
        PlayerAnim.SetLayerWeight(1, activeLayer == 1 ? 1 : 0);
        PlayerAnim.SetLayerWeight(2, activeLayer == 2 ? 1 : 0);
    }

}
