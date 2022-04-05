using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bow : MonoBehaviour//, IPointerUpHandler, IDragHandler, IPointerDownHandler
{

    //V düzgün gerilmiyo onu hallet
    //V aşırı hızlı atmayı engelle
    //Vgüzel renkli silindirler falan yap
    //sağ taraftaki silindire atınca okun topları küçülcek sonraki bölümlerde
    //ok sayısı limitli olsun her bölüm için
    //silindirin bir kısmı renkli olcak o kısım oku içine çekip silindiri küçültücek buda ok saplamayı zorlaştıracak
    //mouseun olduğu yere atıyor, rotasyonunu değiştirilebilir şekilde yap yani rotasyonu

    //sol tarafa yanlış atıyor ama bu önemsiz
    //projede gereksiz şeyleri sil yeni bi proje açıp
    //line collider ile bowun iplerini yap
    public GameObject bowSpring1, bowSpring2;
    GameObject Arrow;
    float old_hipotenus = 0;
    Vector3 ThrustPower;
    float speedRange;
    Vector3 distance;
    float hipotenus=0;
    float decreasement = 10;
    public Button ThrowingButton;
    float SpeedRangeIncrease = 2.6f;
    public GameObject ArrowCountText;
    float firstBowAngle = 0;
    float firstMouseAngle = 0;
    public void onDown() {
        Arrow = Instantiate(Resources.Load<GameObject>("Prefabs/Arrow"), transform.position, Quaternion.Euler(0, 0, 0));
        Arrow.name = "Arrow";
        Arrow.transform.parent = gameObject.transform;
        Arrow.transform.localEulerAngles = new Vector3(0, 180, -90);
        Main.ArrowCount--;
        ArrowCountText.GetComponent<Text>().text = Main.ArrowCount + "";
        distance = Distance();
        firstMouseAngle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        
        firstBowAngle = transform.localEulerAngles.z;
    }
    Vector3 Distance() {
        Vector3 distance;
        Vector3 MousePos;

        Vector3 mouseInputPos = Input.mousePosition;
        mouseInputPos.z = Camera.main.nearClipPlane + 8.7f;
        MousePos = Camera.main.ScreenToWorldPoint(mouseInputPos);
        distance = (MousePos - transform.position) * 2;
        distance.z = 0;
        return distance;
    }
    public void onDrag() {
        /*Vector3[] v = new Vector3[3];
               v[0] = bowSpring1.transform.position;
               v[1] = Arrow.transform.position;
               v[2] = bowSpring2.transform.position;
               GetComponent<LineRenderer>().SetPositions(v);
               Debug.Log(GetComponent<LineRenderer>().GetPosition(1));*/
        //Arrow.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z - 90);
        distance = Distance();

        if (hipotenus < old_hipotenus && speedRange <= 30)
        {
            Arrow.transform.Translate(Vector3.left * (old_hipotenus - hipotenus) / decreasement);
            speedRange += SpeedRangeIncrease;
            decreasement += 1;
            //Arrow.transform.position = new Vector3(Arrow.transform.position.x+(old_hipotenus - hipotenus)/10000f, Arrow.transform.position.y, Arrow.transform.position.z);
        }
        else if (hipotenus > old_hipotenus && speedRange > 0)
        {
            decreasement -= 1;
            speedRange -= SpeedRangeIncrease;
            Arrow.transform.Translate(Vector3.left * (old_hipotenus - hipotenus) / decreasement);


        }
        old_hipotenus = hipotenus;

        hipotenus = Mathf.Sqrt(Mathf.Pow(distance.x, 2) + Mathf.Pow(distance.y, 2));


        float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;

        transform.localEulerAngles = new Vector3(0, 0, firstBowAngle + 2 * (angle - firstMouseAngle));

    }
    public void onUp() {
        ThrustPower = new Vector3(speedRange * distance.x / hipotenus, speedRange * distance.y / hipotenus, 0);
        Arrow.GetComponent<Rigidbody>().velocity = ThrustPower;
        Arrow.GetComponent<Arrow>().bowOut = true;
        
        Arrow.GetComponent<Arrow>().turningSpeed = 10f / (ThrustPower.magnitude+10);
        Arrow.transform.parent = null;
        Arrow.GetComponent<Rigidbody>().isKinematic = false;
        speedRange = 0;
        decreasement = 10;
        old_hipotenus = 0;
    }
    private void Start()
    {
        transform.localEulerAngles = new Vector3(0, 180, 90);
    }

    
    /*
void Update()
{
   if (Input.GetMouseButtonDown(0))
   {
       Arrow = Instantiate(Resources.Load<GameObject>("Prefabs/Arrow"), transform.position, Quaternion.Euler(0, 0, 0));
       Arrow.name = "Arrow";
       Arrow.transform.parent = gameObject.transform;
       Arrow.transform.localEulerAngles = new Vector3(0, 180, -90);
   }
   else if (Input.GetMouseButton(0))
   {

*/
    /*Vector3[] v = new Vector3[3];
    v[0] = bowSpring1.transform.position;
    v[1] = Arrow.transform.position;
    v[2] = bowSpring2.transform.position;
    GetComponent<LineRenderer>().SetPositions(v);
    Debug.Log(GetComponent<LineRenderer>().GetPosition(1));*/
    //Arrow.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z - 90);
    /*Vector3 MousePos;

    Vector3 mouseInputPos = Input.mousePosition;
    mouseInputPos.z = Camera.main.nearClipPlane + 8.7f;
    MousePos = Camera.main.ScreenToWorldPoint(mouseInputPos);
    distance = (MousePos - transform.position) * 2;
    distance.z = 0;

    if (hipotenus < old_hipotenus && speedRange <= 30)
    {
        Arrow.transform.Translate(Vector3.left * (old_hipotenus - hipotenus) / decreasement);
        speedRange += 0.7f;
        decreasement += 1;
        //Arrow.transform.position = new Vector3(Arrow.transform.position.x+(old_hipotenus - hipotenus)/10000f, Arrow.transform.position.y, Arrow.transform.position.z);
    }
    else if (hipotenus > old_hipotenus&&speedRange>0)
    {
        decreasement -= 1;
        speedRange -= 0.7f;
        Arrow.transform.Translate(Vector3.left * (old_hipotenus - hipotenus) / decreasement);


    }
    old_hipotenus = hipotenus;

    hipotenus = Mathf.Sqrt(Mathf.Pow(distance.x, 2) + Mathf.Pow(distance.y, 2));


    float angle = Mathf.Atan2(distance.y, distance.x) * (180 / Mathf.PI);

    transform.localEulerAngles = new Vector3(0, 0, angle + 90);


}
else if (Input.GetMouseButtonUp(0))
{
    ThrustPower = new Vector3(speedRange * distance.x / hipotenus, speedRange * distance.y / hipotenus, 0);
    Arrow.GetComponent<Rigidbody>().velocity = ThrustPower;
    Arrow.GetComponent<Arrow>().bowOut = true;
    Arrow.GetComponent<Arrow>().turningSpeed = 10f/ThrustPower.magnitude;
    Arrow.transform.parent = null;
    Arrow.GetComponent<Rigidbody>().isKinematic = false;
    speedRange = 0;
    decreasement = 10;
    old_hipotenus = 0;
}
}




*/

}
