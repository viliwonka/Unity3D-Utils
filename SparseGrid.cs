using UnityEngine;
using System.Collections.Generic;
using System;

public struct Coords {
    public int x;
    public int y;

    public Coords(int x, int y) {
        this.x = x;
        this.y = y;
    }

    //DIVISOR MUST BE POWER OF TWO
    public Coords Modulo(int divisor) {

        var result = this;

        divisor = divisor - 1;

        result.x &= divisor;
        //result.x = result.x.Modulo(divisor);

        result.y &= divisor;
        
        return result;
    }

    public Coords Quotient(int divisor) {

        var result = this;


        if (result.x >= 0)
            result.x = result.x / divisor;
        else
            result.x = result.x / divisor - 1;


        if (result.y >= 0)
            result.y = result.y / divisor;
        else
            result.y = result.y / divisor - 1;

        return result;
    }

    public static Coords Infinity() {
        return new Coords(int.MaxValue, int.MaxValue);
    }
    //Enakost
    public static bool operator ==(Coords a, Coords b) {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(Coords a, Coords b) {
        return a.x != b.x || a.y != b.y;
    }
}

public class CoordsEqualityComparer : IEqualityComparer<Coords> {
    public bool Equals(Coords a, Coords b) {
        return !(a.x != b.x || a.y != b.y);
    }

    public int GetHashCode(Coords coord) {
        return 419 + coord.x * 863 + coord.y * 149;
    }
}

public class SparseGrid<T>  {

    //Must be power of two OR IT WILL FUCK THINGS UP!!
    int chunkSize = 64; //64x64 chunk

    Dictionary<Coords, T[][]> chunks;
    
    public SparseGrid(){
        chunks = new Dictionary<Coords, T[][]>(new CoordsEqualityComparer());
    }

    T[][] EmptyChunk() {

        T[][] chunk = new T[chunkSize][];

        for (int i = 0; i < chunkSize; i++) 
            chunk[i] = new T[chunkSize];
        
        return chunk;
    }

    //Used for caching purposes;
    Coords previousQuotient = Coords.Infinity();
    T[][] previousChunk;
    public T this[Coords c] {

        set {

            T[][] chunk;

            //Quotient
            var cQ = c.Quotient(chunkSize);

            
            if (cQ == previousQuotient) {
                chunk = previousChunk;

            }
            else {

                //Access Dictionary
                if (!chunks.ContainsKey(cQ))
                    chunks[cQ] = chunk = EmptyChunk();
                else
                    chunk = chunks[cQ];

                previousQuotient = cQ;
                previousChunk = chunk;
            }
            //Remainder
            var cR = c.Modulo(chunkSize);

            //Ostanek se uporabi kot index na chunku
            try {
                chunk[cR.x][cR.y] = value;
            }
            catch {
                Debug.Log(cR.x + " " + cR.y + ": " + c.y);

                throw new Exception();
            }
        }

        get {
            //Quotient
            var cQ = c.Quotient(chunkSize);
            
            T[][] chunk;
            
            if (cQ == previousQuotient) {
                chunk = previousChunk;

            }
            else {

                //Access Dictionary
                if (!chunks.ContainsKey(cQ))
                    return default(T);
                else
                    chunk = chunks[cQ];

                previousQuotient = cQ;
                previousChunk = chunk;
            }

            if (chunk == null) return default(T);
                
            //Remainder
            var cR = c.Modulo(chunkSize);

            //Ostanek se uporabi kot index na chunku
            return chunk[cR.x][cR.y];
        }
    }
}