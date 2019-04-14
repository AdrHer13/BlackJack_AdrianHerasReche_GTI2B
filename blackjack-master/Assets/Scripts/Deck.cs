using System;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;

    //Comentario general: ha sido necesario repartirle primero al dealer a la hora de calcular probabilidades.
    //  Si no eran probabilidades erroneas
    //No sabía si tenía que tener en cuenta la carta oculta del dealer como oculta o como mostrada, así que he hecho ambos códigos

    //necesitamos un nuevo array para barajar de mismo tamaño que values
    public int[] values = new int[52];
    int cartasRestante = 52;
    int cardIndex = 0;

    //para hacer la banca
    public Text MoneyText;
    public Text BetMoneyText;
    public int Money;
    public int Bet;
    public Button button10;
    public Button button100;
    public Button button1000;
    public Button button10000;
    public Button buttonAllIn;

    private void Awake()
    {
        InitCardValues();
    }

    private void Start()
    {
        ShuffleCards();
        Money = 1000;
        StartGame();
        MoneyText.text = Money.ToString();
    }

    private void InitCardValues()
    {
        for (int i = 0; i < 52; i++)
        {
            if (i == 0 || i == 13 || i == 26 || i == 39) values[i] = 11;
            if (i == 1 || i == 14 || i == 27 || i == 40) values[i] = 2;
            if (i == 2 || i == 15 || i == 28 || i == 41) values[i] = 3;
            if (i == 3 || i == 16 || i == 29 || i == 42) values[i] = 4;
            if (i == 4 || i == 17 || i == 30 || i == 43) values[i] = 5;
            if (i == 5 || i == 18 || i == 31 || i == 44) values[i] = 6;
            if (i == 6 || i == 19 || i == 32 || i == 45) values[i] = 7;
            if (i == 7 || i == 20 || i == 33 || i == 46) values[i] = 8;
            if (i == 8 || i == 21 || i == 34 || i == 47) values[i] = 9;
            if (i == 9 || i == 22 || i == 35 || i == 48) values[i] = 10;
            if (i == 10 || i == 23 || i == 36 || i == 49) values[i] = 10;
            if (i == 11 || i == 24 || i == 37 || i == 50) values[i] = 10;
            if (i == 12 || i == 25 || i == 38 || i == 51) values[i] = 10;
        }
    }

    private void ShuffleCards()
    {

        int random;
        int aux;
        Sprite aux_sprite;

        for (int i = 0; i < 52; i++)
        {
            random = UnityEngine.Random.Range(0, 52);
            aux = values[i];
            aux_sprite = faces[i];
            values[i] = values[random];
            faces[i] = faces[random];

            values[random] = aux;
            faces[random] = aux_sprite;
        }

    }

    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
        }
        CalculateProbabilities();

        if (dealer.GetComponent<CardHand>().points == player.GetComponent<CardHand>().points && dealer.GetComponent<CardHand>().points == 21 && player.GetComponent<CardHand>().points == 21)
        {
            finalMessage.text = "Tie with Blackjacks, no one wins";
            dealer.GetComponent<CardHand>().InitialToggle();

            hitButton.interactable = false;
            stickButton.interactable = false;

            Money += Bet;
            MoneyText.text = Money.ToString();
            Bet = 0;
            BetMoneyText.text = Bet.ToString();
        }
        else if (dealer.GetComponent<CardHand>().points == 21)
        {
            //Jugador pierde
            finalMessage.text = "Blackjack" + Environment.NewLine + "Dealer wins";

            dealer.GetComponent<CardHand>().InitialToggle();

            hitButton.interactable = false;
            stickButton.interactable = false;

            Bet = 0;
            BetMoneyText.text = Bet.ToString();

        }
        else if (player.GetComponent<CardHand>().points == 21)
        {
            //Dealer pierde
            finalMessage.text = "Blackjack" + Environment.NewLine + "Player wins";

            dealer.GetComponent<CardHand>().InitialToggle();

            hitButton.interactable = false;
            stickButton.interactable = false;

            Money += 2 * Bet;
            MoneyText.text = Money.ToString();
            Bet = 0;
            BetMoneyText.text = Bet.ToString();
        }

        Bet = 0;
        BetMoneyText.text = Bet.ToString();
        if (Money > 0)
        {
            RaiseBet(10);
        }
    }

    private void CalculateProbabilities()
    {
        //Probabilidad de dealer con más puntuacion
        float probabilidadDealerMasPuntuacion = 0.0f;
        int casosFavorables = 0;
        int[] numeros = new int[10];
        for (int i = 0; i < numeros.Length; i++)
        {
            numeros[i] = i + 1;
        }

        if (dealer.GetComponent<CardHand>().cards.ToArray().Length > 1)
        {
            foreach (int numero in numeros)
            {
                if (numero > (player.GetComponent<CardHand>().points - dealer.GetComponent<CardHand>().cards.ToArray()[1].GetComponent<CardModel>().value))
                {
                    int cantidadCartas = 4;
                    if (numero == 10)
                    {
                        cantidadCartas = 16;
                    }
                    foreach (GameObject f in player.GetComponent<CardHand>().cards)
                    {
                        if (f.GetComponent<CardModel>().value == numero)
                        {
                            cantidadCartas--;
                        }
                    }
                    foreach (GameObject f in dealer.GetComponent<CardHand>().cards)
                    {
                        if (f.GetComponent<CardModel>().value == numero)
                        {
                            cantidadCartas--;
                        }
                    }
                    /*if (dealer.GetComponent<CardHand>().cards.ToArray()[1].GetComponent<CardModel>().value == numero)
                    {
                        cantidadCartas--;
                    }*/

                    casosFavorables += cantidadCartas;
                }
            }
        }
        probabilidadDealerMasPuntuacion = (float)casosFavorables / cartasRestante * 100;
        if (probabilidadDealerMasPuntuacion >= 100)
        {
            probMessage.text = "Probabilty that the dealer has more points: " + "100%" + Environment.NewLine;
        }
        else
        {
            probMessage.text = "Probabilty that the dealer has more points: " + Convert.ToInt16(probabilidadDealerMasPuntuacion).ToString() + "%" + Environment.NewLine;
        }

        // probabilidad de pasarse
        float probabilidadClavarla = 0.0f;
        int casosClavarla = 0;

        if (dealer.GetComponent<CardHand>().cards.ToArray().Length > 1)
        {
            foreach (int numero in numeros)
            {
                if (numero >= (17 - player.GetComponent<CardHand>().points) && numero <= (21 - player.GetComponent<CardHand>().points))
                {
                    int cantidadCartas = 4;
                    if (numero == 10)
                    {
                        cantidadCartas = 16;
                    }
                    foreach (GameObject f in player.GetComponent<CardHand>().cards)
                    {
                        if (f.GetComponent<CardModel>().value == numero)
                        {
                            cantidadCartas--;
                        }
                    }
                    foreach (GameObject f in dealer.GetComponent<CardHand>().cards)
                    {
                        if (f.GetComponent<CardModel>().value == numero)
                        {
                            cantidadCartas--;
                        }
                    }
                    /*if (dealer.GetComponent<CardHand>().cards.ToArray()[1].GetComponent<CardModel>().value == numero)
                    {
                        cantidadCartas--;
                    }*/
                    casosClavarla += cantidadCartas;
                }
            }
        }
        probabilidadClavarla = (float)casosClavarla / cartasRestante * 100;
        if (probabilidadClavarla >= 100)
        {
            probMessage.text += "Probability to have between 17 & 21 point at drawing: " + "100%" + Environment.NewLine;
        }
        else
        {
            probMessage.text += "Probability to have between 17 & 21 point at drawing: " + Convert.ToInt16(probabilidadClavarla).ToString() + "%" + Environment.NewLine;
        }

        // probabilidad de pasarse
        float probabilidadPasarse = 0.0f;
        int casosPasarse = 0;

        if (dealer.GetComponent<CardHand>().cards.ToArray().Length > 1)
        {
            foreach (int numero in numeros)
            {
                if (numero > (21 - player.GetComponent<CardHand>().points))
                {
                    int cantidadCartas = 4;
                    if (numero == 10)
                    {
                        cantidadCartas = 16;
                    }
                    foreach (GameObject f in player.GetComponent<CardHand>().cards)
                    {
                        if (f.GetComponent<CardModel>().value == numero)
                        {
                            cantidadCartas--;
                        }
                    }
                    if (dealer.GetComponent<CardHand>().cards.ToArray()[1].GetComponent<CardModel>().value == numero)
                    {
                        cantidadCartas--;
                    }
                    casosPasarse += cantidadCartas;
                }
            }
        }
        probabilidadPasarse = (float)casosPasarse / cartasRestante * 100;
        if (probabilidadPasarse >= 100)
        {
            probMessage.text += "Probability to bust: " + "100%";
        }
        else
        {
            probMessage.text += "Probability to bust: " + Convert.ToInt16(probabilidadPasarse).ToString() + "%";
        }
    }

    void PushDealer()
    {
        dealer.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]);
        cardIndex++;
        cartasRestante--;
    }

    void PushPlayer()
    {
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        cartasRestante--;
        CalculateProbabilities();
    }

    public void Hit()
    {
        //Repartimos carta al jugador
        PushPlayer();

        if (player.GetComponent<CardHand>().points > 21)
        {
            //Jugador pierde porque se pasa
            finalMessage.text = "Player has busted, you lose";

            dealer.GetComponent<CardHand>().InitialToggle();

            hitButton.interactable = false;
            stickButton.interactable = false;

            Bet = 0;
            BetMoneyText.text = Bet.ToString();
        }

        if (player.GetComponent<CardHand>().points == 21)
        {
            //Jugador pierde porque se pasa
            finalMessage.text = "Player has 21, you win";

            dealer.GetComponent<CardHand>().InitialToggle();

            hitButton.interactable = false;
            stickButton.interactable = false;

            Money += 2 * Bet;
            MoneyText.text = Money.ToString();
            Bet = 0;
            BetMoneyText.text = Bet.ToString();
        }

        button10.interactable = false;
        button100.interactable = false;
        button1000.interactable = false;
        button10000.interactable = false;
        buttonAllIn.interactable = false;
    }

    public void Stand()
    {
        //TODO: comprobacion de ases

        dealer.GetComponent<CardHand>().InitialToggle();

        while (dealer.GetComponent<CardHand>().points < 17)
        {
            //Si el dealer tiene 17 o más deja de pedir cartas
            PushDealer();
        }

        if (dealer.GetComponent<CardHand>().points > player.GetComponent<CardHand>().points && dealer.GetComponent<CardHand>().points <= 21 && player.GetComponent<CardHand>().points <= 21)
        {
            finalMessage.text = "Dealer wins";

            hitButton.interactable = false;
            stickButton.interactable = false;

            Bet = 0;
            BetMoneyText.text = Bet.ToString();
        }
        else if (dealer.GetComponent<CardHand>().points < player.GetComponent<CardHand>().points && player.GetComponent<CardHand>().points <= 21 && dealer.GetComponent<CardHand>().points <= 21)
        {
            finalMessage.text = "Player wins";

            hitButton.interactable = false;
            stickButton.interactable = false;

            Money += 2 * Bet;
            MoneyText.text = Money.ToString();
            Bet = 0;
            BetMoneyText.text = Bet.ToString();
        }
        else if (dealer.GetComponent<CardHand>().points > 21)
        {
            finalMessage.text = "Dealer has busted, you win";

            hitButton.interactable = false;
            stickButton.interactable = false;

            Money += 2 * Bet;
            MoneyText.text = Money.ToString();
            Bet = 0;
            BetMoneyText.text = Bet.ToString();
        }
        else if (dealer.GetComponent<CardHand>().points == player.GetComponent<CardHand>().points)
        {
            finalMessage.text = "Tie, no one wins";

            hitButton.interactable = false;
            stickButton.interactable = false;

            Money += Bet;
            MoneyText.text = Money.ToString();
            Bet = 0;
            BetMoneyText.text = Bet.ToString();
        }

        if (Money <= 0)
        {
            playAgainButton.interactable = false;
        }

        checkBetButtons();
    }

    public void Button10OnClick()
    {
        if (Money >= 10)
        {
            RaiseBet(10);
        }
        else
        {
            checkBetButtons();
        }
    }

    public void Button100OnClick()
    {
        if (Money >= 100)
        {
            RaiseBet(100);
        }
        else
        {
            checkBetButtons();
        }
    }

    public void Button1000OnClick()
    {
        if (Money >= 1000)
        {
            RaiseBet(1000);
        }
        else
        {
            checkBetButtons();
        }
    }

    public void Button10000OnClick()
    {
        if (Money >= 10000)
        {
            RaiseBet(10000);
        }
        else
        {
            checkBetButtons();
        }
    }

    public void ButtonAllInOnClick()
    {
            RaiseBet(Money);
    }

    public void RaiseBet(int raise)
    {
        Bet += raise;
        Money -= raise;
        BetMoneyText.text = Bet.ToString();
        MoneyText.text = Money.ToString();
        checkBetButtons();
    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        checkBetButtons();
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();
        cartasRestante = 52;
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }

    public void checkBetButtons()
    {
        if (Money >= 100000)
        {
            button10.interactable = true;
            button100.interactable = true;
            button1000.interactable = true;
            button10000.interactable = true;
        }
        else if (Money >= 10000)
        {
            button10.interactable = true;
            button100.interactable = true;
            button1000.interactable = true;
            button10000.interactable = true;
        }
        else if (Money >= 1000)
        {
            button10.interactable = true;
            button100.interactable = true;
            button1000.interactable = true;
            button10000.interactable = false;
        }
        else if (Money >= 100)
        {
            button10.interactable = true;
            button100.interactable = true;
            button1000.interactable = false;
            button10000.interactable = false;
        }
        else if (Money >= 10)
        {
            button10.interactable = true;
            button100.interactable = false;
            button1000.interactable = false;
            button10000.interactable = false;
        }
        else if (Money <= 0)
        {
            button10.interactable = false;
            button100.interactable = false;
            button1000.interactable = false;
            button10000.interactable = false;
            buttonAllIn.interactable = false;
        }
    }

}
