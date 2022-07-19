using UnityEngine;

public class PhisicaforCube : MonoBehaviour
{
    public GameObject restartButton, explosionEffects;
    bool _collison = false;

     private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cube" && !_collison)
        {
            for (int i = collision.transform.childCount-1; i>=0; i--)
            {
                Transform child = collision.transform.GetChild(i);
                child.gameObject.AddComponent<Rigidbody>();
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(70f,
                                                                          Vector3.up,5f);
                child.SetParent(null);
            }
            restartButton.SetActive(true);
            Camera.main.transform.localPosition -= new Vector3(0, 0, 3f);
            Camera.main.gameObject.AddComponent<ShakeCamera>();

            if (PlayerPrefs.GetString("music") != "No")
            {
                GetComponent<AudioSource>().Play();
            }

            GameObject newEffect = Instantiate(explosionEffects, new Vector3(
                                   collision.contacts[0].point.x,
                                   collision.contacts[0].point.y,
                                   collision.contacts[0].point.z),
                                   Quaternion.identity) as GameObject;
            Destroy(newEffect, 2.5f);
            Destroy(collision.gameObject);
            _collison = true;
        }
    }
}
