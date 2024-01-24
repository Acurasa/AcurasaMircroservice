﻿using Auction_Service.Data;
using Auction_Service.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Reflection.Metadata.Ecma335;

namespace Auction_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AuctionDbContext _context;
        public AuctionsController(IMapper mapper, AuctionDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
        {
            var auctions = await _context.Auctions
                .Include(x => x.Item)
                .OrderBy(x => x.Item.Make)
                .ToListAsync();

            return _mapper.Map<List<AuctionDto>>(auctions);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDto>> GetAuctionById()
        {
            var auction = await _context.Auctions
                .Include(x => x.Item)
                .FirstOrDefaultAsync();

            return auction is null ? NotFound() : _mapper.Map<AuctionDto>(auction);
        }

        [HttpPost]
        public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
        {
            var auction = _mapper.Map<Auction>(auctionDto);
            auction.Seller = "TEST"; //placeholder for Identity Server

            _context.Auctions.Add(auction);

            if (!(await _context.SaveChangesAsync() > 0))
            {
                BadRequest($"Could not save Changes to the DB, Object : {nameof(auction)}");
            }

            return CreatedAtAction(nameof(GetAuctionById), new { auction.Id }, _mapper.Map<AuctionDto>(auction));

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuction(Guid id,UpdateAuctionDto updateAuctionDto)
        {
            var auction = await _context.Auctions
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x=> x.Id == id);
            
            if (auction is null)
            {
                return NotFound();
            }

            // TODO: Identity Check

            auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
            auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
            auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
            auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
            auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

            if (!(await _context.SaveChangesAsync() > 0))
            {
                BadRequest($"Could not save Changes to the DB, Object : {nameof(auction)}");
            }

            return Ok();

        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteAuction(Guid id)
        {
            var auction = await _context.Auctions.FirstOrDefaultAsync(x => x.Id == id);
            if ( auction is null ) { return NotFound(); }


            // TODO: Identity Check

            _context.Auctions.Remove(auction);

            if (!(await _context.SaveChangesAsync() > 0))
            {
                BadRequest($"Could not save Changes to the DB, Object : {nameof(auction)}");
            }

            return Ok();
        }



    }
}
