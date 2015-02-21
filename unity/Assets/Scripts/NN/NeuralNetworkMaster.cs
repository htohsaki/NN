using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Ai.NeuralNetwork {
	/**
	 * ニューラルネットワーク全体を管理する
	 * */
	public class NeuralNetworkMaster  {

		public NeuralNetworkLayer inputLayer;
		public NeuralNetworkLayer hiddenLayer;
		public NeuralNetworkLayer outputLayer;


		public NeuralNetworkMaster(){
			this.inputLayer = new NeuralNetworkLayer ();
			this.hiddenLayer = new NeuralNetworkLayer ();
			this.outputLayer = new NeuralNetworkLayer ();

		}

		/**
		 * 各ニューロン層を指定したニューロン数で初期化する
		 * */
		public void Initialize(int nodesInputNum, int nodesHiddenNum, int nodesOutputNum){

			this.inputLayer.numberOfNodes = nodesInputNum;
			this.inputLayer.numberOfChildNodes = nodesHiddenNum;
			this.inputLayer.numberOfParentNodes = 0;
			this.inputLayer.Initialize (nodesInputNum, null, this.hiddenLayer);
			this.inputLayer.RandomizeWeights ();

			this.hiddenLayer.numberOfNodes = nodesHiddenNum;
			this.hiddenLayer.numberOfChildNodes = nodesOutputNum;
			this.hiddenLayer.numberOfParentNodes = nodesInputNum;
			this.hiddenLayer.Initialize (nodesHiddenNum, this.inputLayer, this.hiddenLayer);
			this.hiddenLayer.RandomizeWeights ();

			this.outputLayer.numberOfNodes = nodesOutputNum;
			this.outputLayer.numberOfChildNodes = 0;
			this.outputLayer.numberOfParentNodes = nodesHiddenNum;
			this.outputLayer.Initialize (nodesOutputNum, this.hiddenLayer, null);

		}

		public void CleanUp(){
			this.inputLayer.CleanUp ();
			this.hiddenLayer.CleanUp ();
			this.outputLayer.CleanUp ();
		}

		public void SetInput(int i, float value){
			if (i < 0) {
				throw new Exception ("SetInputのIndexは0以上でないといけない@SetInput i=" + i);
			} else if (i >= this.inputLayer.numberOfNodes) {
				throw new Exception ("SetInputのIndexは" + this.inputLayer.numberOfNodes + "未満でないといけない@SetInput i=" + i);
			}
			this.inputLayer.neuronValues [i] = value;

		}

		public float GetOutput(int i){
			if (i < 0) {
				throw new Exception ("GetOutputのIndexは0以上でないといけない@GetOutput i=" + i);
			} else if (i >= this.outputLayer.numberOfNodes) {
				throw new Exception ("GetOutputのIndexは" + this.outputLayer.numberOfNodes + "未満でないといけない@GetOutput i=" + i);
			}
			return this.outputLayer.neuronValues [i];

		}

		public void SetDesiredOutput(int i, float value){
			if (i < 0) {
				throw new Exception ("SetDesiredOutputのIndexは0以上でないといけない@SetDesiredOutput i=" + i);
			} else if (i >= this.outputLayer.numberOfNodes) {
				throw new Exception ("SetDesiredOutputのIndexは" + this.outputLayer.numberOfNodes + "未満でないといけない@SetDesiredOutput i=" + i);
			}
			this.outputLayer.desiredValues [i] = value;
		}

		public void FeedForward(){
			this.inputLayer.CalculateNeuronValues ();
			this.hiddenLayer.CalculateNeuronValues ();
			this.outputLayer.CalculateNeuronValues ();
		}

		public void BackPropagate(){
			this.outputLayer.CalculateErrors ();
			this.hiddenLayer.CalculateErrors ();
			this.hiddenLayer.AdjustWeights ();
			this.inputLayer.AdjustWeights ();
		}

		/**
		 * 出力値が最も高い出力ニューロンを特定する（総取り形式で活性化する出力を判断する場合）
		 * */
		public int GetMaxOutputId(){
			float maxval = this.outputLayer.neuronValues [0];
			int id = 0;
			for (int i = 1; i < this.outputLayer.numberOfNodes; i++) {
				if (this.outputLayer.neuronValues [i] > maxval) {
					maxval = this.outputLayer.neuronValues [i];
					id = i;
				}
			}
			return id;
		}

		/**
		 * 特定の出力セットに関するエラーを計算うる
		 * */
		public float CalculateError(){
			float error = 0;

			for (int i = 0; i < this.outputLayer.numberOfNodes; i++) {
				error += Mathf.Pow (this.outputLayer.neuronValues [i] - this.outputLayer.desiredValues [i], 2);
			}

			error = error / this.outputLayer.numberOfNodes;
			return error;
		}

		public void SetLearningRate(float rate){
			this.inputLayer.learningRate = rate;
			this.outputLayer.learningRate = rate;
			this.hiddenLayer.learningRate = rate;
		}

		public void SetLinearOutput(bool useLinear){
			this.inputLayer.linearOutput = useLinear;
			this.outputLayer.linearOutput = useLinear;
			this.hiddenLayer.linearOutput = useLinear;
		}

		public void SetMomentum(bool useMomentum, float factor){
			this.inputLayer.SetMomentum (useMomentum, factor);
			this.outputLayer.SetMomentum (useMomentum, factor);
			this.hiddenLayer.SetMomentum (useMomentum, factor);
		}

		public void DumpData(string fileName){
			string s = "----------------------------------\n";

			s += "Input Layer\n";
			s += "----------------------------------\n";
			s += "\n";
			s += "Node Values:\n";
			s += "\n";

			for (int i = 0; i < this.inputLayer.numberOfNodes; i++) {
				s += "(" + i + ") = " + this.inputLayer.neuronValues[i] + "\n";
			}
			s += "\n";
			s += "Weights:\n";
			s += "\n";

			for (int i = 0; i < this.inputLayer.numberOfNodes; i++) {
				for (int j = 0; j < this.inputLayer.numberOfChildNodes; j++) {
					s += "(" + i + "," + j + ") = " + this.inputLayer.weights[i,j] + "\n";
				}
			}
			s += "\n";
			s += "Bias Weights:\n";
			s += "\n";

			for (int i = 0; i < this.inputLayer.numberOfChildNodes; i++) {
				s += "(" + i + ")" + this.inputLayer.biasWeights[i] + "\n";
			}

			s += "\n";
			s += "\n";
			s += "----------------------------------\n";
			s += "Hidden Layer\n";
			s += "----------------------------------\n";
			s += "\n";

			s += "Weights:\n";
			s += "\n";

			for (int i = 0; i < this.hiddenLayer.numberOfNodes; i++) {
				for (int j = 0; j < this.hiddenLayer.numberOfChildNodes; j++) {
					s += "(" + i + "," + j + ") = " + this.hiddenLayer.weights[i, j] + "\n";
				}
			}
			s += "\n";
			s += "Bias Weights:\n";
			s += "\n";
			for (int i = 0; i < this.hiddenLayer.numberOfChildNodes; i++) {
				s += "(" + i + ")" + this.hiddenLayer.biasWeights[i] + "\n";
			}

			s += "\n";
			s += "\n";
			s += "----------------------------------\n";
			s += "Output Layer\n";
			s += "----------------------------------\n";
			s += "\n";

			s += "Node Values:\n";
			s += "\n";

			for (int i = 0; i < this.outputLayer.numberOfNodes; i++) {
				s += "(" + i + ") = " + this.outputLayer.neuronValues[i] + "\n";
			}


			fileName = Application.persistentDataPath + @"/" + fileName;
			StreamWriter saveWriter = new StreamWriter (fileName, 
				false, System.Text.Encoding.GetEncoding ("utf-8"));
			saveWriter.NewLine = "\n";
			saveWriter.Write (s);
			saveWriter.Close ();
		}
	}
}
