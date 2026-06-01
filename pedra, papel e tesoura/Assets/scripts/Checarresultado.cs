using UnityEngine;

public class Checarresultado : MonoBehaviour
{

    public int olharquemvenceu(GameManager.escolhadojogador escolhaJogador1, GameManager.escolhadojogador escolhaJogador2)
    { //lógica do jogo para conclcuir quem venceu o round, retornando 0 para empate, 1 para jogador 1 e 2 para jogador 2.
      if (escolhaJogador1 == escolhaJogador2){return 0;} // empate técnico
        else if ((escolhaJogador1 == GameManager.escolhadojogador.Pedra && escolhaJogador2 == GameManager.escolhadojogador.Tesoura) ||
                 (escolhaJogador1 == GameManager.escolhadojogador.Papel && escolhaJogador2 == GameManager.escolhadojogador.Pedra) ||
                 (escolhaJogador1 == GameManager.escolhadojogador.Tesoura && escolhaJogador2 == GameManager.escolhadojogador.Papel))
        {
            return 1; // Jogador 1 vence
        }
        else
        {
            return 2; // Jogador 2 vence
        }
    }
}
