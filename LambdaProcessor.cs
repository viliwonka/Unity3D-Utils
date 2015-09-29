using UnityEngine;
using System.Collections.Generic;
using System;

public class LambdaProcessor : MonoBehaviour {


    List<Action> lambdas;

    int counter = 0;

    int rateCounter = 0;
    int rate = 1;

    void Awake() {

        lambdas = new List<Action>() { () => {} };
    }

	void FixedUpdate () {

        rateCounter += 1;
        rateCounter %= rate;

        //if rate == 1, this executes every frame
        if (rateCounter == 0) {

            counter += 1;
            counter %= lambdas.Count;

            lambdas[counter]();
        }
	}

    //rate = 1 - every frame something executes
    //rate = 2 - every second frame something executes
    //rate = 3 - every third frame something executes etc
    public void SetRate(int rate){
        this.rate = rate;
    }

    //Adds other action to execute
    //Just used to build Lambda processor
    public void AddLambda(Action lambda) {

        lambdas.Add(lambda);
    }
} 