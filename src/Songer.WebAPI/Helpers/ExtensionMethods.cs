using Songer.WebAPI.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Songer.WebAPI.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<UserDto> ToUserDtosList(this IEnumerable<User> users)
        {
            var usersDto = new List<UserDto>();

            foreach (var user in users)
            {
                usersDto.Add(new UserDto(user));
            }

            return usersDto;
        }

        public static IEnumerable<SongDto> ToSongDtosList(this IEnumerable<Song> songs)
        {
            var songsDto = new List<SongDto>();

            foreach (var song in songs)
            {
                songsDto.Add(new SongDto(song));
            }

            return songsDto;
        }

        public static IEnumerable<PlaylistDto> ToPlaylistDtosList(this IEnumerable<Playlist> playlists)
        {
            var playlistsDto = new List<PlaylistDto>();

            foreach (var playlist in playlists)
            {
                playlistsDto.Add(new PlaylistDto(playlist));
            }

            return playlistsDto;
        }
    }
}
