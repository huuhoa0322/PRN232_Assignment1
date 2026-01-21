using HE186716_DoHuuHoa_SE1884_NET_A01_BE.DTOs;
using HE186716_DoHuuHoa_SE1884_NET_A01_BE.Models;
using HE186716_DoHuuHoa_SE1884_NET_A01_BE.Repositories;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_BE.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;

    public TagService(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<IEnumerable<TagDto>> GetAllAsync()
    {
        var tags = await _tagRepository.SearchAsync(null);
        return tags.Select(MapToDto);
    }

    public async Task<TagDto?> GetByIdAsync(int id)
    {
        var tag = await _tagRepository.GetByIdAsync(id);
        return tag == null ? null : MapToDto(tag);
    }

    public async Task<IEnumerable<TagDto>> SearchAsync(string? keyword)
    {
        var tags = await _tagRepository.SearchAsync(keyword);
        return tags.Select(MapToDto);
    }

    public async Task<TagDto> CreateAsync(CreateTagDto dto)
    {
        var newId = await _tagRepository.GenerateNewIdAsync();

        var tag = new Tag
        {
            TagId = newId,
            TagName = dto.TagName,
            Note = dto.Note
        };

        await _tagRepository.AddAsync(tag);
        return MapToDto(tag);
    }

    public async Task<TagDto?> UpdateAsync(int id, UpdateTagDto dto)
    {
        var tag = await _tagRepository.GetByIdAsync(id);
        if (tag == null)
            return null;

        tag.TagName = dto.TagName;
        tag.Note = dto.Note;

        await _tagRepository.UpdateAsync(tag);
        return MapToDto(tag);
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var tag = await _tagRepository.GetByIdAsync(id);
        if (tag == null)
            return (false, "Tag not found");

        // Check if tag is used in any articles
        if (await _tagRepository.IsUsedInArticlesAsync(id))
            return (false, "Cannot delete tag that is used in news articles");

        await _tagRepository.DeleteAsync(tag);
        return (true, "Tag deleted successfully");
    }

    private TagDto MapToDto(Tag tag)
    {
        return new TagDto
        {
            TagId = tag.TagId,
            TagName = tag.TagName,
            Note = tag.Note,
            ArticleCount = tag.NewsArticles?.Count ?? 0
        };
    }
}
