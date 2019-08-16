using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public GameObject[] tampinhas;
    public GameObject tampinhaSelecionada;
    public LayerMask tampinhaSelecionadaLayer;

    private List<GameObject> tampinhasNaoSelecionadas = new List<GameObject>();

	// Use this for initialization
	void Start () {
        tampinhas[0].GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5, ForceMode2D.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null) {
                if (hit.collider.gameObject.CompareTag(GameConstants.TAMPINHA)) {
                    ResetTampinhas();
                    tampinhaSelecionada = hit.collider.gameObject;
                    tampinhaSelecionada.tag = GameConstants.TAMPINHA_SELECIONADA;
                    tampinhaSelecionada.layer = LayerMask.NameToLayer(GameConstants.TAMPINHA_SELECIONADA_LAYER);
                    UpdateTampinhasNaoSelecionadas();
                    //Debug.Log("Selecionada: " + hit.collider.gameObject.name);
                    // hit.collider.attachedRigidbody.AddForce(Vector2.up);
                }
            } else {
                ResetTampinhas();
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if (tampinhaSelecionada != null) {
                float power = CalculatePower();
                //Debug.Log("Lançar tampinha com power: " + power);

                // calcular direcao
                Vector2 direction = CalculateLaunchDirection();
                tampinhaSelecionada.GetComponent<Rigidbody2D>().AddForce(direction * power, ForceMode2D.Impulse);
            }
        }
        CheckRaycastCollision();

        // Chamados no update apenas para debug
        CalculatePower();
        CalculateLaunchDirection();
    }

    void CheckRaycastCollision() {
        if (tampinhasNaoSelecionadas.Count == 2) {
            Vector3 start = new Vector3(tampinhasNaoSelecionadas[0].transform.position.x, tampinhasNaoSelecionadas[0].transform.position.y, tampinhasNaoSelecionadas[0].transform.position.z);
            Vector3 end = new Vector3(tampinhasNaoSelecionadas[1].transform.position.x, tampinhasNaoSelecionadas[1].transform.position.y, tampinhasNaoSelecionadas[1].transform.position.z);

            //Debug.DrawLine(start, end, Color.red);

            RaycastHit2D hit = Physics2D.Linecast(start, end, tampinhaSelecionadaLayer);
            if (hit.collider != null) {
                if (hit.collider.gameObject.CompareTag(GameConstants.TAMPINHA_SELECIONADA)) {
                    //Debug.Log("Entre tampinhas: " + hit.collider.name);
                }
            }
        }
    }

    void ResetTampinhas() {
        tampinhaSelecionada = null;
        for (int i = 0; i < tampinhas.Length; i++) {
            tampinhas[i].tag = GameConstants.TAMPINHA;
            tampinhas[i].layer = LayerMask.NameToLayer("Default");
        }
    }
    
    void UpdateTampinhasNaoSelecionadas() {
        tampinhasNaoSelecionadas.Clear();
        for (int i = 0; i < tampinhas.Length; i++) {
            if (tampinhas[i].tag != GameConstants.TAMPINHA_SELECIONADA) {
                tampinhasNaoSelecionadas.Add(tampinhas[i]);
                //Debug.Log("Nao selecionadas: " + tampinhas[i].name);
            }
        }
    }

    float CalculatePower() {
        float distance = 0f;

        if (tampinhaSelecionada != null) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            Vector2 tampinhaPos2D = new Vector2(tampinhaSelecionada.transform.position.x, tampinhaSelecionada.transform.position.y);

            Debug.DrawLine(tampinhaPos2D, mousePos2D, Color.green);
            distance = Vector2.Distance(tampinhaPos2D, mousePos2D);

            //Debug.Log(distance);
        }
        return distance;
    }

    Vector2 CalculateLaunchDirection() {
        Vector2 direction = Vector2.up;

        if (tampinhaSelecionada != null) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            Vector2 tampinhaPos2D = new Vector2(tampinhaSelecionada.transform.position.x, tampinhaSelecionada.transform.position.y);

            direction = new Vector2(tampinhaPos2D.x - mousePos2D.x, tampinhaPos2D.y - mousePos2D.y);
            Debug.DrawLine(tampinhaPos2D, direction, Color.blue);
            direction = Vector2.up;
        }
        return direction;
    }
}
