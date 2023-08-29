using System;
using System.Collections.Generic;
using System.Text;
using SocketGameProtocol;
using SocketServer.DAO;
using System.Data.SqlClient;

namespace SocketServer.Controller
{
    public class SongControl
    {
        private SongData songData;
        public SongControl()
        {
            songData = new SongData();
        }

        //歌曲商店中搜索
        public MainPack SearchSongs(MainPack pack)
        {
            pack.Songs.Clear();
            if ((pack.Searchsongpack.SongName == "") && (pack.Searchsongpack.Author == ""))
            {
                pack.Songs.Add(songData.GetAllSongs(pack.User.Userid));
                pack.Authors.Add(songData.GetAllAuthors());
                if (pack.Songs != null && pack.Authors != null)
                {
                    pack.Returncode = ReturnCode.Succeed;
                }
                else
                {
                    pack.Returncode = ReturnCode.Fail;
                }

            }
            else if ((pack.Searchsongpack.SongName == "") && (pack.Searchsongpack.Author != ""))
            {
                pack.Songs.Add(songData.SearchSongsByAuthor(pack.Searchsongpack.Author));
                if (pack.Songs != null)
                {
                    pack.Returncode = ReturnCode.Succeed;
                }
                else
                {
                    pack.Returncode = ReturnCode.Fail;
                }
            }
            else if ((pack.Searchsongpack.SongName != "") && (pack.Searchsongpack.Author == ""))
            {
                pack.Songs.Add(songData.SerchSongsByName(pack.Searchsongpack.SongName));
                if (pack.Songs != null)
                {
                    pack.Returncode = ReturnCode.Succeed;
                }
                else
                {
                    pack.Returncode = ReturnCode.Fail;
                }
            }
            else if ((pack.Searchsongpack.SongName != "") && (pack.Searchsongpack.Author != ""))
            {
                Song[] songs = songData.SearchSongsByNameAndAuthor(pack.Searchsongpack.SongName, pack.Searchsongpack.Author);
                if(songs!=null)
                    pack.Songs.Add(songs);
                if (pack.Songs != null)
                {
                    pack.Returncode = ReturnCode.Succeed;
                }
                else
                {
                    pack.Returncode = ReturnCode.Fail;
                }

            }
            return pack;
        }
        public MainPack SearchSongsByAuthor(MainPack pack)
        {
            pack.Songs.Add(songData.SearchSongsByAuthor(pack.Searchsongpack.Author));
            if (pack.Songs != null)
            {
                pack.Returncode = ReturnCode.Succeed;
            }
            else
            {
                pack.Returncode = ReturnCode.Fail;
            }
            return pack;
        }
        
    }
}
