using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region variables_publicas
    public Camara camara;
    public RawImage imgCam;
    public Text[] text;
    #endregion
    #region variables_privadas
    private Camaras camaras;
    private bool cargado = false;
    #endregion

    void Awake()
    {
        StartCoroutine(GetQuestions("https://servizos.meteogalicia.gal/mgrss/observacion/jsonCamaras.action"));
    }
     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&cargado==true)
        {
            cargado = false;
            camara = camaras.listaCamaras[Random.Range(0, camaras.listaCamaras.Count)];
            StartCoroutine(DownloadImage(camara.imaxeCamara));
        }
    }

    IEnumerator GetQuestions(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                LoadJson(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log("No se puede conectar con la api");
            }
        }
    }

    public void LoadJson(string jsonString)
    {
        camaras = JsonUtility.FromJson<Camaras>(jsonString);
        camara = camaras.listaCamaras[Random.Range(0,camaras.listaCamaras.Count)];
        StartCoroutine(DownloadImage(camara.imaxeCamara));
    }


    IEnumerator DownloadImage(string MediaUrl)
    {
       using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(MediaUrl))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                imgCam.texture = (Texture)((DownloadHandlerTexture)webRequest.downloadHandler).texture;

                text[0].text = camara.concello;
                text[1].text = camara.dataUltimaAct;
                text[2].text = camara.idConcello.ToString();
                text[3].text = camara.identificador.ToString();
                text[4].text = camara.lat.ToString();
                text[5].text = camara.lon.ToString();
                text[6].text = camara.nomeCamara;
                text[7].text = camara.provincia;
                cargado = true;
            }
            else
            {
                Debug.Log("No se puede conectar con la api");
            }
        }
            }
}
