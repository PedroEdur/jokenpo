using UnityEngine;

public class Turnos : MonoBehaviour
{
    public int jogadorAtual = 1;

    public void TrocarTurno()
    {
        if (jogadorAtual == 1)
        {
            jogadorAtual = 2;
        }
        else
        {
            jogadorAtual = 1;
        }
    }

    public void ReiniciarTurno()
    {
        jogadorAtual = 1;
    }
}