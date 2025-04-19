using AutoMapper;
using DATA.DTO;
using DATA.Interface;

namespace CORE.ItemServices
{
    public class ItemService : IItemService

    {
        private readonly IITemRepository _iTemRepository;
        private readonly IMapper _mapper;

        public ItemService(IITemRepository iTemRepository, IMapper mapper)
        {
            _iTemRepository = iTemRepository;
            _mapper = mapper;
        }
        public async Task<ResponseDto> SearchItemsAsync(string keyword)
        {
            var response = new ResponseDto();

            if(string.IsNullOrWhiteSpace(keyword))
            {
                response.StatusCode = 400;
                response.Message = "Keyword cannot be empty.";
                return response;
            }

            try
            {
                var matchedItem = await _iTemRepository.SearchItemsAsync(keyword);

                if(!matchedItem.Any())
                {
                    response.StatusCode = 404;
                    response.Message = "No reports found matching the search criteria.";
                    return response;
                }

                var mappedItem = matchedItem
                    .Select(item => _mapper.Map<ItemResponseDto>(item))
                    .ToList();

                response.StatusCode = 200;
                response.Message = "Search successful.";
                response.Result = mappedItem;
                return response;
            }
            catch(Exception ex)
            {
                response.StatusCode = 500;
                response.Message = $"An error occurred during the search: {ex.Message}";
                return response;
            }
        }
    }
}
