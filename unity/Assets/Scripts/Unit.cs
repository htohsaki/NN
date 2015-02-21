using UnityEngine;
using System.Collections;

using Ai.NeuralNetwork;

public class Unit : MonoBehaviour {

	NeuralNetworkMaster brain;


	void Start () {
		brain.Initialize (4, 3, 3);
		brain.SetLearningRate (0.2f);
		brain.SetMomentum (true, 0.9f);

		TrainBrain ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	float[,] trainingSet = new float[,]{
		//仲間の数,	ヒットポイント,	敵が交戦中,	距離,	追跡,	集結,	逃避
		{0.0f,	1.0f,	0.0f,	0.2f,	0.9f,	0.1f,	0.1f},
		{0.0f,	1.0f,	1.0f,	0.2f,	0.9f,	0.1f,	0.1f},
		{0.0f,	1.0f,	0.0f,	0.8f,	0.1f,	0.1f,	0.1f},
		{0.1f,	0.5f,	0.0f,	0.2f,	0.9f,	0.1f,	0.1f},
		{0.0f,	0.25f,	1.0f,	0.5f,	0.1f,	0.9f,	0.1f},
		{0.0f,	0.2f,	1.0f,	0.2f,	0.1f,	0.1f,	0.9f},
		{0.3f,	0.2f,	0.0f,	0.2f,	0.9f,	0.1f,	0.1f},
		{0.0f,	0.2f,	0.0f,	0.3f,	0.1f,	0.9f,	0.1f},
		{0.0f,	1.0f,	0.0f,	0.2f,	0.1f,	0.9f,	0.1f},
		{0.0f,	1.0f,	1.0f,	0.6f,	0.1f,	0.1f,	0.1f},
		{0.0f,	1.0f,	0.0f,	0.8f,	0.1f,	0.9f,	0.1f},
		{0.1f,	0.2f,	0.0f,	0.2f,	0.1f,	0.1f,	0.9f},
		{0.0f,	0.25f,	1.0f,	0.5f,	0.1f,	0.1f,	0.9f},
		{0.0f,	0.6f,	0.0f,	0.2f,	0.1f,	0.1f,	0.9f}
	};


	public void TrainBrain(){

		float error = 1.0f;
		int c = 0;

		this.brain.DumpData ("preTraining.txt");

		while ((error > 0.05) && c < 50000) {
			error = 0.0f;
			c++;

			for (int i = 0; i < this.trainingSet.GetLength(0); i++) {
				this.brain.SetInput (0, this.trainingSet [i, 0]);
				this.brain.SetInput (1, this.trainingSet [i, 1]);
				this.brain.SetInput (2, this.trainingSet [i, 2]);
				this.brain.SetInput (3, this.trainingSet [i, 3]);

				this.brain.SetDesiredOutput(0, this.trainingSet[i,4]);
				this.brain.SetDesiredOutput(1, this.trainingSet[i,5]);
				this.brain.SetDesiredOutput(2, this.trainingSet[i,6]);

				this.brain.FeedForward ();
				error += this.brain.CalculateError ();
				this.brain.BackPropagate ();
			}

			error = error / 14.0f;
		}

		this.brain.DumpData ("postTraining.txt");

	}

}
