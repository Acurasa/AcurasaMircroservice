using Auction_Service.Data;
using Auction_Service.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Auction_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Route("api/auctions")]
    public class AuctionsController : ControllerBase
    {
        private readonly AuctionDbContext _context;
        private readonly IMapper _mapper;
        public AuctionsController(AuctionDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
        {
            var auctions = await _context.Auctions
                .Include(x=> x.Item)
                .OrderBy(x=>x.Item.Make)
                .ToListAsync();

            return _mapper.Map<List<AuctionDto>>(auctions);
        }


        [HttpGet("{id}")]
        
        public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
        {
            var auction = await _context.Auctions
                .Include(x=> x.Item)
                .FirstOrDefaultAsync();
            if (auction == null)
            {
                return NotFound();
            }
            return _mapper.Map<AuctionDto>(auction);
        }
    }
}
