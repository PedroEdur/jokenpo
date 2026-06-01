using UnityEngine;

public class GameManager : MonoBehaviour
{

    public enum GameState {Resultado, Revelação, Esperandojogador1, Esperandojogador2, Proximoround, Fim}

    public GameState EstadoAtual;

    public int pontosJogador1;
    public int pontosJogador2;

    public int roundAtual = 1;
    public int maxRound = 5; 

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EstadoAtual = Esperandojogador1;
    }

    public void Mudançadeestado(GameState EstadoNovo)
    {
        EstadoAtual = EstadoNovo;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
