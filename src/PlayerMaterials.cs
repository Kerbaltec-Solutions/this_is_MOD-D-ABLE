// keeps track of the materials a player has gathered 

using System;
using System.IO;
public class PlayerMaterials{
    public int Food{get; set;} = 4;
    public int Money{get; set;} = 4;

    public void SetMaterials(int[] input, TIM.main game){
        Food = input[0];
        Money = input[1];
    }

    public void IncMaterials(int[] input, TIM.main game){
        Food += input[0];
        Money += input[1];
    }

    public void IncMaterialsSave(int[] input, TIM.main game){
        Food += input[0];
        Money += input[1];

        if(Food<0){
            Console.WriteLine("Food too low!");
            Food+=input[0];
            throw new System.ArgumentOutOfRangeException("Food too low!");
        }
        if(Money<0){
            Console.WriteLine("Money too low!");
            Money+=input[1];
            throw new System.ArgumentOutOfRangeException("Money too low!");
        }
    }

    public void PrintMaterials(TIM.main game){
        Console.WriteLine("Food: {0}", Food);
        Console.WriteLine("Money: {0}", Money);
    }
}