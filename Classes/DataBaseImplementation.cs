using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicBeePlugin;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Deployment.Internal;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace MusicBeePlugin
{
    public class DataBaseImplementation
    {
        //My Fields
        internal MusicBeeApiInterface mbApiInterface;
        internal string albumArtist = null;
        public MySqlConnection connection;
        int albumArtistID = 0;
        string albumArtistName = null;
        int songID = 0;
        string songName = null;
        int albumID = 0;
        string albumName = null;
        string trackNum = null;
        int artistID = 0;
        string artistsName = null;
        string artistName = null;
        string trimmedArtist = null;
        int noOfArtists = 0;

        public DataBaseImplementation(Plugin.MusicBeeApiInterface mApi)
        {
            mbApiInterface = mApi;

        }

        public DataBaseImplementation(string albumartist, string song, string album, string artist, string trackNo)
        {
            albumArtist = albumartist;
            songName = song;
            albumName = album;
            trackNum = trackNo;
            artistsName = artist;
        }

        private void CheckAlbumArtist()
        {
            //Connect to the DB
            string connectionString = "Server=localhost;Database=mysql;User=root;Password=password;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            //Checking the database for an Album Artist with the same name
            string sqlAlbumArtist = "SELECT * FROM AlbumArtist WHERE AlbumArtistName = @artist_MB";
            MySqlCommand commandAlbumArtist = new MySqlCommand(sqlAlbumArtist, connection);
            commandAlbumArtist.Parameters.AddWithValue("@artist_MB", albumArtist);
            MySqlDataReader readerAlbumArtist = commandAlbumArtist.ExecuteReader();

            if (readerAlbumArtist.Read())
            {
                albumArtistID = Convert.ToInt16(readerAlbumArtist["AlbumArtistID"]);
                albumArtistName = Convert.ToString(readerAlbumArtist["AlbumArtistName"]);
                readerAlbumArtist.Close();
            }
            else
            {
                readerAlbumArtist.Close();

                //Insertion of non existent artist in DataBase
                string inserterAlbumArtistSTG = "INSERT INTO AlbumArtist(AlbumArtistName) VALUES (@artist_MB)";
                MySqlCommand inserterAlbumArtist = new MySqlCommand(inserterAlbumArtistSTG, connection);
                inserterAlbumArtist.Parameters.AddWithValue("@artist_MB", albumArtist);
                inserterAlbumArtist.ExecuteNonQuery();

                //Checking ID number for new album artist on DB
                MySqlDataReader readerAlbumArtist2 = commandAlbumArtist.ExecuteReader();

                if (readerAlbumArtist2.Read())
                {
                    albumArtistID = Convert.ToInt16(readerAlbumArtist2["AlbumArtistID"]);
                    albumArtistName = Convert.ToString(readerAlbumArtist2["AlbumArtistName"]);
                    readerAlbumArtist2.Close();
                }

            }
            connection.Close();
        }

        private void CheckSong()
        {
            //This data is necessary for the song on the DB
            CheckAlbumArtist();

            //Connect to the DB
            string connectionString = "Server=localhost;Database=mysql;User=root;Password=password;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

             //Checking the database for a track with the same name
             string sqlSong = "SELECT * FROM Song WHERE SongName = @Song_MB";
             MySqlCommand commandSong = new MySqlCommand(sqlSong, connection);
             commandSong.Parameters.AddWithValue("@Song_MB", songName);
             MySqlDataReader readerSong = commandSong.ExecuteReader();

             if (readerSong.Read())
             {
                 songID = Convert.ToInt16(readerSong["SongID"]);
                 songName = Convert.ToString(readerSong["SongName"]);
                 readerSong.Close();
             }

             else
             {
                 readerSong.Close();

                 //Insertion of non existent track in DataBase
                 string inserterStringSong = "INSERT INTO Song(SongName, AlbumArtistID) VALUES (@song_MB, @artist_artist_id)";
                 MySqlCommand inserterSong = new MySqlCommand(inserterStringSong, connection);
                 inserterSong.Parameters.AddWithValue("@song_MB", songName);
                 inserterSong.Parameters.AddWithValue("@artist_artist_id", albumArtistID);
                 inserterSong.ExecuteNonQuery();

                 //Checking ID number for new Song on DB
                 MySqlDataReader readerSong2 = commandSong.ExecuteReader();
                 if (readerSong2.Read())
                 {
                     songID = Convert.ToInt16(readerSong2["SongID"]);
                     songName = Convert.ToString(readerSong2["SongName"]);
                     readerSong2.Close();
                 }

                 else
                 {
                     readerSong2.Close();
                 }
             }
            connection.Close();
        }

        private void CheckAlbum()
        {
            //Connect to the DB
            string connectionString = "Server=localhost;Database=mysql;User=root;Password=password;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            //This data is necessary for the album on the DB
            CheckAlbumArtist();

            //Checking the database for an album with the same name
            string sqlAlbum = "SELECT * FROM Album WHERE AlbumName = @Album_MB";
            MySqlCommand commandAlbum = new MySqlCommand(sqlAlbum, connection);
            commandAlbum.Parameters.AddWithValue("@Album_MB", albumName);
            MySqlDataReader readerAlbum = commandAlbum.ExecuteReader();
            if (readerAlbum.Read())
            {
                albumID = Convert.ToInt16(readerAlbum["AlbumID"]);
                albumName = Convert.ToString(readerAlbum["AlbumName"]);
                readerAlbum.Close();
            }

            else
            {
                readerAlbum.Close();

                //Insertion of non existent Album in DataBase
                string inserterStringAlbum = "INSERT INTO Album(AlbumArtistID, AlbumName) VALUES (@Alvum_Artist_ID_DB, @Album_Name_DB)";
                MySqlCommand inserterAlbum = new MySqlCommand(inserterStringAlbum, connection);
                inserterAlbum.Parameters.AddWithValue("@Alvum_Artist_ID_DB", albumArtistID);
                inserterAlbum.Parameters.AddWithValue("@Album_Name_DB", albumName);
                inserterAlbum.ExecuteNonQuery();

                //Checking ID number for new Album on DB
                MySqlDataReader readerAlbum2 = commandAlbum.ExecuteReader();
                if (readerAlbum2.Read())
                {
                    albumID = Convert.ToInt16(readerAlbum2["AlbumID"]);
                    albumName = Convert.ToString(readerAlbum2["AlbumName"]);
                    readerAlbum2.Close();
                }

            }
            connection.Close();
        }

        private void CheckAlbum_Song()
        { 
            //Connect to the DB
            string connectionString = "Server=localhost;Database=mysql;User=root;Password=password;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            //This Data is necessary
            CheckSong();
            CheckAlbum();
            CheckAlbumArtist();

            //Checking the database for this connection between Album and Song
            MySqlCommand commandAlbum_Song = new MySqlCommand("SELECT * FROM Album_Song WHERE SongID = @SongID_DB AND AlbumID = @AlbumID_DB", connection);
            commandAlbum_Song.Parameters.AddWithValue("@SongID_DB", songID);
            commandAlbum_Song.Parameters.AddWithValue("@AlbumID_DB", albumID);
            MySqlDataReader readerAlbum_Song = commandAlbum_Song.ExecuteReader();
            if (readerAlbum_Song.Read())
            {
                readerAlbum_Song.Close();
            }
            else
            {
                readerAlbum_Song.Close();

                //Insertion of non existent Connection in DataBase
                MySqlCommand inserterAlbum_Song = new MySqlCommand("INSERT INTO Album_Song VALUES (@Album_Song_AlbumID, @Album_Song_SongID, @TrackNo, @AlbumArtistID)", connection);
                inserterAlbum_Song.Parameters.AddWithValue("@Album_Song_AlbumID", albumID);
                inserterAlbum_Song.Parameters.AddWithValue("@Album_Song_SongID", songID);
                inserterAlbum_Song.Parameters.AddWithValue("@TrackNo", trackNum);
                inserterAlbum_Song.Parameters.AddWithValue("@AlbumArtistID", albumArtistID);
                inserterAlbum_Song.ExecuteNonQuery();
                MySqlDataReader readerAlbum_Song2 = commandAlbum_Song.ExecuteReader();
                try
                {
                    readerAlbum_Song2.Read();
                    readerAlbum_Song2.Close();
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void CheckArtists()
        {
            string[] artists = artistsName.Split(';');
            noOfArtists = artists.Length;
            foreach (string artist in artists)
            {
                trimmedArtist = artist.Trim();

                //Connect to the DB
                string connectionString = "Server=localhost;Database=mysql;User=root;Password=password;";
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                //Checking the database for an Album Artist with the same name
                string sqlArtist = "SELECT * FROM Artists WHERE ArtistName = @artist_MB";
                MySqlCommand commandArtist = new MySqlCommand(sqlArtist, connection);
                commandArtist.Parameters.AddWithValue("@artist_MB", trimmedArtist);
                MySqlDataReader readerArtist = commandArtist.ExecuteReader();

                if (readerArtist.Read())
                {
                    artistID = Convert.ToInt16(readerArtist["ArtistID"]);
                    artistName = Convert.ToString(readerArtist["ArtistName"]);
                    readerArtist.Close();
                    CheckArtists_Song();
                }
                else
                {
                    readerArtist.Close();

                    //Insertion of non existent artist in DataBase
                    string inserterArtistSTG = "INSERT INTO Artists(ArtistName) VALUES (@artist_MB)";
                    MySqlCommand inserterArtist = new MySqlCommand(inserterArtistSTG, connection);
                    inserterArtist.Parameters.AddWithValue("@artist_MB", trimmedArtist);
                    inserterArtist.ExecuteNonQuery();

                    //Checking ID number for new album artist on DB
                    MySqlDataReader readerArtist2 = commandArtist.ExecuteReader();

                    if (readerArtist2.Read())
                    {
                        artistID = Convert.ToInt16(readerArtist2["ArtistID"]);
                        artistName = Convert.ToString(readerArtist2["ArtistName"]);
                        readerArtist2.Close();
                        CheckArtists_Song();
                    }
                }
                connection.Close();
            }
        }

        private void CheckArtists_Song()
        {
            //Connect to the DB
            string connectionString = "Server=localhost;Database=mysql;User=root;Password=password;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            //This Data is necessary
            CheckSong();

            //Checking the database for this connection between Artist and Song
            MySqlCommand commandArtist_Song = new MySqlCommand("SELECT * FROM Artists_Song WHERE SongID = @SongID_DB AND ArtistID = @ArtistID_DB", connection);
            commandArtist_Song.Parameters.AddWithValue("@SongID_DB", songID);
            commandArtist_Song.Parameters.AddWithValue("@ArtistID_DB", artistID);
            MySqlDataReader readerArtist_Song = commandArtist_Song.ExecuteReader();
            if (readerArtist_Song.Read())
            {
                readerArtist_Song.Close();
            }
            else
            {
                readerArtist_Song.Close();

                //Insertion of non existent Connection in DataBase
                MySqlCommand inserterArtist_Song = new MySqlCommand("INSERT INTO Artists_Song VALUES (@Artists_Song_ArtistID, @Artists_Song_SongID, @NoOfArtists)", connection);
                inserterArtist_Song.Parameters.AddWithValue("@Artists_Song_ArtistID", artistID);
                inserterArtist_Song.Parameters.AddWithValue("@Artists_Song_SongID", songID);
                inserterArtist_Song.Parameters.AddWithValue("@NoOfArtists", noOfArtists);
                inserterArtist_Song.ExecuteNonQuery();
                MySqlDataReader readerArtist_Song2 = commandArtist_Song.ExecuteReader();
                try
                {
                    readerArtist_Song2.Read();
                    readerArtist_Song2.Close();
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void MakeScrobble()
        { 
            CheckSong();
            CheckAlbum();
            CheckAlbum_Song();
            CheckArtists();

            //Connect to the DB
            string connectionString = "Server=localhost;Database=mysql;User=root;Password=password;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            try
            {
                //Insertion of a Scrobble in DB
                MySqlCommand inserterScrobble = new MySqlCommand("INSERT INTO scrobble (AlbumArtistID, AlbumID, SongID, DateScrobble, TimeScrobble)   VALUES (@AlbumArtistID_DB, @AlbumID_DB, @SongID_DB, @Date, @Time)", connection);
                inserterScrobble.Parameters.AddWithValue("@AlbumArtistID_DB", albumArtistID);
                inserterScrobble.Parameters.AddWithValue("@AlbumID_DB", albumID);
                inserterScrobble.Parameters.AddWithValue("@SongID_DB", songID);
                inserterScrobble.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                inserterScrobble.Parameters.AddWithValue("@Time", DateTime.Now.ToString("HH:mm"));
                inserterScrobble.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
            }
            connection.Close();
        }
    }
}