import React, { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { AlertCircle, CheckCircle, Tags, Pencil, Trash2, PlusCircle } from "lucide-react";
import { genreService } from "../../services/genreService";
import { Genre } from "../../types";

interface GenreFormData {
  genreName: string;
  genreDescription: string;
}

const GenreManagerPage: React.FC = () => {
  const [genres, setGenres] = useState<Genre[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [editingGenre, setEditingGenre] = useState<Genre | null>(null);

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<GenreFormData>();

  useEffect(() => {
    loadGenres();
  }, []);

  const loadGenres = async () => {
    try {
      const res = await genreService.getAllGenres();
      setGenres(res.data);
    } catch (err) {
      console.error(err);
    }
  };

  const onSubmit = async (data: GenreFormData) => {
    setLoading(true);
    setError("");
    try {
      if (editingGenre) {
        await genreService.updateGenre({
          genreId: editingGenre.genreId,
          genreName: data.genreName,
          genreDescription: data.genreDescription,
        });
        setSuccess("Genre updated successfully!");
      } else {
        await genreService.createGenre(data);
        setSuccess("Genre created successfully!");
      }
      reset();
      setEditingGenre(null);
      loadGenres();
    } catch (err: any) {
      setError(err.response?.data?.message || "Something went wrong");
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (genre: Genre) => {
    setEditingGenre(genre);
    reset({
      genreName: genre.genreName,
      genreDescription: genre.genreDescription,
    });
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm("Are you sure you want to delete this genre?")) return;
    try {
      await genreService.deleteGenre(id);
      setGenres((prev) => prev.filter((g) => g.genreId !== id));
    } catch (err: any) {
      setError(err.response?.data?.message || "Failed to delete genre");
    }
  };

  return (
    <div className="max-w-4xl mx-auto space-y-8">
      <div className="text-center">
        <Tags className="w-12 h-12 text-amber-600 mx-auto" />
        <h1 className="mt-4 text-3xl font-bold text-gray-900">Manage Genres</h1>
        <p className="mt-2 text-gray-600">
          Add, edit, or delete book genres
        </p>
      </div>

      <div className="bg-white rounded-xl shadow-lg border border-amber-100 p-8">
        {error && (
          <div className="mb-6 bg-red-50 border border-red-200 rounded-lg p-4 flex items-center space-x-2 text-red-700">
            <AlertCircle className="w-5 h-5" />
            <span>{error}</span>
          </div>
        )}

        {success && (
          <div className="mb-6 bg-green-50 border border-green-200 rounded-lg p-4 flex items-center space-x-2 text-green-700">
            <CheckCircle className="w-5 h-5" />
            <span>{success}</span>
          </div>
        )}

        {/* Genre Form */}
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
          <div>
            <label className="block text-sm font-medium text-gray-700">
              Genre Name *
            </label>
            <input
              {...register("genreName", { required: "Genre name is required" })}
              type="text"
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent"
              placeholder="Enter genre name"
            />
            {errors.genreName && (
              <p className="mt-1 text-sm text-red-600">
                {errors.genreName.message}
              </p>
            )}
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700">
              Description *
            </label>
            <textarea
              {...register("genreDescription", {
                required: "Description is required",
              })}
              rows={3}
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent resize-none"
              placeholder="Enter genre description"
            />
            {errors.genreDescription && (
              <p className="mt-1 text-sm text-red-600">
                {errors.genreDescription.message}
              </p>
            )}
          </div>

          <div className="flex justify-end space-x-4 pt-6">
            {editingGenre && (
              <button
                type="button"
                onClick={() => {
                  setEditingGenre(null);
                  reset();
                }}
                className="px-6 py-2 text-gray-700 bg-gray-100 rounded-lg hover:bg-gray-200 transition-colors"
              >
                Cancel Edit
              </button>
            )}
            <button
              type="submit"
              disabled={loading}
              className="px-6 py-2 bg-amber-600 text-white rounded-lg hover:bg-amber-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-amber-500 disabled:opacity-50 disabled:cursor-not-allowed transition-colors flex items-center space-x-2"
            >
              {loading ? (
                <span>Saving...</span>
              ) : editingGenre ? (
                <>
                  <Pencil className="w-4 h-4" />
                  <span>Update Genre</span>
                </>
              ) : (
                <>
                  <PlusCircle className="w-4 h-4" />
                  <span>Add Genre</span>
                </>
              )}
            </button>
          </div>
        </form>
      </div>

      {/* Genre List */}
      <div className="bg-white rounded-xl shadow-md border border-gray-200 p-6">
        <h2 className="text-lg font-semibold mb-4">Existing Genres</h2>
        <ul className="space-y-3">
          {genres.map((genre) => (
            <li
              key={genre.genreId}
              className="flex justify-between items-center border p-3 rounded-lg"
            >
              <div>
                <p className="font-semibold">{genre.genreName}</p>
                <p className="text-sm text-gray-500">
                  {genre.genreDescription}
                </p>
              </div>
              <div className="flex gap-2">
                <button
                  onClick={() => handleEdit(genre)}
                  className="px-3 py-1 bg-blue-100 text-blue-700 rounded-lg hover:bg-blue-200 flex items-center space-x-1"
                >
                  <Pencil className="w-4 h-4" />
                  <span>Edit</span>
                </button>
                <button
                  onClick={() => handleDelete(genre.genreId)}
                  className="px-3 py-1 bg-red-100 text-red-700 rounded-lg hover:bg-red-200 flex items-center space-x-1"
                >
                  <Trash2 className="w-4 h-4" />
                  <span>Delete</span>
                </button>
              </div>
            </li>
          ))}
        </ul>
        {genres.length === 0 && (
          <p className="text-gray-500 text-center">No genres found.</p>
        )}
      </div>
    </div>
  );
};

export default GenreManagerPage;
