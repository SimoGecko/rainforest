// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class LevelBuilder : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public MeshPiece[] meshpieces;
    public Texture2D map;
    public float highOffset = 4f;
    public float threshold = .05f;
    

    // private


    // references
    GameObject level;
	
	
	// --------------------- BASE METHODS ------------------
	void Start () {

	}
	
	void Update () {
        
	}

    // --------------------- CUSTOM METHODS ----------------


    // commands
    [ContextMenu("Generate Level")]
    void GenerateLevel() {
        if (level != null) DestroyImmediate(level);
        level = new GameObject("Level");
        GeneratePieces();
        level.transform.position = new Vector3(-map.width / 4, 0, -map.height / 4);
    }

    void GeneratePieces() {
        foreach(MeshPiece mp in meshpieces) {

            for (int rot = 0; rot < 360; rot += 90) {
                //rotate pieceTex
                Texture2D pieceTex = Rotate(TexFromSprite(mp.image), rot);

                //scan to find image correspondence
                for (int x = 0; x <= map.width - pieceTex.width; x++) {
                    for (int y = 0; y <= map.height - pieceTex.height; y++) {

                        if(EqualsImage(pieceTex, map, new Vector2(x,y))) {
                            //instantiate piece with rotation 
                            Vector3 position = new Vector3(x + pieceTex.width/2, 0, y + pieceTex.height/2); // consider center of image
                            if (mp.high) position.y = highOffset*2;
                            GameObject newPiece = Instantiate(mp.prefab, position/2, Quaternion.identity); // 2 pixels = 1 unit
                            newPiece.transform.Rotate(Vector3.up * (rot + mp.rotOffset));
                            newPiece.transform.parent = level.transform;
                        } 
                    }
                }
            }
        }
    }



    // queries
    Texture2D TexFromSprite(Sprite s) {
        Texture2D croppedTexture = new Texture2D((int)s.rect.width, (int)s.rect.height);
        var pixels = s.texture.GetPixels((int)s.textureRect.x,(int)s.textureRect.y,(int)s.textureRect.width,(int)s.textureRect.height);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();
        return croppedTexture;
    }

    Texture2D Rotate(Texture2D t, int rot) {
        //rotates image by rot degrees
        Texture2D result = t;
        for (int i = 0; i < rot/90; i++) {
            result = RotateCW(result);
        }
        return result;
    }

    Texture2D RotateCW(Texture2D t) {
        Texture2D result = new Texture2D(t.height, t.width);
        for (int x = 0; x < result.width; x++) {
            for (int y = 0; y < result.height; y++) {
                Color c = t.GetPixel(t.width-1 -y, x); // SURE?
                result.SetPixel(x, y, c);
            }
        }
        result.Apply();
        return result;
    }

    bool EqualsImage(Texture2D original, Texture2D cut, Vector2 offset) {
        for (int x = 0; x < original.width; x++) {
            for (int y = 0; y < original.height; y++) {
                Color a = original.GetPixel(x, y);
                Color b = cut.GetPixel(x + (int)offset.x, y + (int)offset.y);
                if (!SimilarColor(a, b)) return false;
                //if (a != b) return false; // they are similar (numerical error)
            }
        }
        return true;
    }

    bool SimilarColor(Color a, Color b) {
        float dist = Mathf.Pow(a.r - b.r, 2) + Mathf.Pow(a.g - b.g, 2) + Mathf.Pow(a.b - b.b, 2);
        return dist <= threshold;
    }



    // other
    [System.Serializable]
    public struct MeshPiece {
        public Sprite image;
        public GameObject prefab;
        public bool high;
        public int rotOffset;
    }

}