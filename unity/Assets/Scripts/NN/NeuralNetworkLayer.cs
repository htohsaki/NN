using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ai.NeuralNetwork {

	/**
	 * ニューラルネットワークの層
	 * */
	public class NeuralNetworkLayer  {
		private static System.Random randomSeed = new System.Random ();

		//この層のニューロンの数
		public int numberOfNodes;
		//この層の子の層のニューロンの数
		public int numberOfChildNodes;
		//この層の親の層のニューロンの数
		public int numberOfParentNodes;

		//親層と子層を接続うるノードの重み
		public float[,] weights;
		//重みの値の調整用
		public float[,] weightChanges;
		//このニューロンに対して計算された活性化値を格納
		public float[] neuronValues;
		//この層のニューロンの望ましい値を格納する
		public float[] desiredValues;
		//この層のニューロンに関するエラーを格納する
		public float[] errors;
		//この層のニューロンに接続するバイアスの重み
		public float[] biasWeights;
		//この層のニューロンに接続するバイアスの値 通常は+1か-1で重みで調整
		public float[] biasValues;
		//重みを調整する学習率
		public float learningRate;

		//活性化関数に、線形活性化関数を使うかどうか、falseだとロジスティクス活性化関数を使う
		public bool linearOutput = false;
		//重みを調整するときに、モーメンタムを使うかどうか
		private bool useMomentum = true;
		//モーメンタム因数
		private float momentumFactor = 0.9f;
		//この層の親の層 
		public NeuralNetworkLayer parentLayer = null;
		//この層の子の層　
		public NeuralNetworkLayer childLayer = null;

		public NeuralNetworkLayer(){

		}

		public NeuralNetworkLayer Initialize(int numberOfNodes, NeuralNetworkLayer parent, NeuralNetworkLayer child){
			this.numberOfNodes = numberOfNodes;

			if (parent != null) {
				this.parentLayer = parent;
			}
			if (child != null) {
				this.childLayer = child;

				this.neuronValues = new float[numberOfNodes];
				this.desiredValues = new float[numberOfNodes];
				this.errors = new float[numberOfNodes];

				this.weights = new float[numberOfNodes, numberOfChildNodes];
				this.weightChanges = new float[numberOfNodes, numberOfChildNodes];

				this.biasValues = new float[numberOfChildNodes];
				this.biasWeights = new float[numberOfChildNodes];

				this.initAr (this.neuronValues, 0);
				this.initAr (this.desiredValues, 0);
				this.initAr (this.errors, 0);
				this.initDoubleAr (this.weights, 0);
				this.initDoubleAr (this.weightChanges, 0);
				this.initAr (this.biasValues, -1);
				this.initAr (this.biasWeights, 0);
			}

			return this;
		}

		public void CleanUp(){
			this.neuronValues = null;
			this.desiredValues = null;
			this.errors = null;
			this.weights = null;
			this.weightChanges = null;
			this.biasValues = null;
			this.biasWeights = null;

		}

		/**
		 * weights, biasWeights にランダムに重みをつける
		 * */
		public void RandomizeWeights(){

			for (int i = 0; i < this.weights.GetLength(0); i++) {
				for (int j = 0; i < this.weights.GetLength(1); j++) {
					this.weights [i, j] = randomSeed.Next (2) - 1;
				}
			}
			for (int i = 0; i < this.biasWeights.Length; i++) {
				this.biasWeights[i] = randomSeed.Next (2) - 1;
			}

		}

		/**
		 * エラーを計算する
		 * */
		public void CalculateErrors(){
			if (this.childLayer == null) { 
				//出力層の場合は出力層用のエラー計算
				for (int i = 0; i < numberOfNodes; i++) {
					this.errors [i] = (this.desiredValues [i] - this.neuronValues [i]) * this.neuronValues [i] * (1.0f - this.neuronValues [i]);
				}
			} else if (this.parentLayer == null) { //入力層の場合はエラーは0
				for (int i = 0; i < numberOfNodes; i++) {
					this.errors [i] = 0.0f;
				}
			} else { //隠し層
				for (int i = 0; i < numberOfNodes; i++) {
					float sum = 0.0f;
					for (int j = 0; j < numberOfChildNodes; j++) {
						sum += this.childLayer.errors [j] * this.weights [i,j];
					}
					this.errors [i] = sum * this.neuronValues [i] - (1.0f - this.neuronValues [i]);
				}
			}
		}

		/**
		 * 接続の重みを調整する
		 * 出力層は重みの調整を行わない
		 * */
		public void AdjustWeights(){
			float dw = 0.0f;
			if (childLayer != null) {
				for (int i = 0; i < numberOfNodes; i++) {
					for (int j = 0; j < numberOfChildNodes; j++) {
						dw = learningRate * this.childLayer.errors [j] * this.neuronValues [i];
						if (useMomentum) {
							weights [i, j] += dw + momentumFactor * weightChanges [i,j];
							weightChanges [i,j] = dw;
						} else {
							weights [i, j] += dw;
						}
					}
				}

				for (int j = 0; j < this.numberOfChildNodes; j++) {
					biasWeights [j] += learningRate * this.childLayer.errors [j] * biasValues [j];
				}

			}

		}

		/**
		 * ニューロンへの最終入力から、層のニューロンの活性化または値を計算する
		 * */
		public void CalculateNeuronValues(){
			float x = 0.0f;
			if (this.parentLayer != null) {
				for (int i = 0; i < numberOfNodes; i++) {
					x = 0;
					for (int j = 0; j < numberOfParentNodes; j++) {
						x += this.parentLayer.neuronValues [j] * this.parentLayer.weights [j, i];
					}
					x += this.parentLayer.biasValues [i] * this.parentLayer.biasWeights [i];

					if ((this.childLayer == null) && linearOutput) {
						this.neuronValues [i] = x;
					} else {
						this.neuronValues [i] = 1.0f / (1 + Mathf.Exp (-1));
					}
				}

			}
		}

		/**
		 * モーメンタムの設定
		 * */
		public void SetMomentum(bool useMomentum, float factor){
			this.useMomentum = useMomentum;
			this.momentumFactor = factor;
		}

		private void initAr(float[] ar, float val){
			for (int i = 0; i < ar.Length; i++) {
				ar [i] = val;
			}
		}

		private void initDoubleAr(float[,] ar, float val){
			for (int i = 0; i < ar.GetLength(0); i++) {
				for (int j = 0; j < ar.GetLength(1); j++) {
					ar [i , j] = val;
				}
			}
		}


	}

}
