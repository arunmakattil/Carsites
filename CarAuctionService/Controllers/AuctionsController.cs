using AutoMapper;
using CarAuctionService.Data;
using CarAuctionService.DTOs;
using CarAuctionService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionService.Controllers
{
    [ApiController]
    [Route("api/auctions")]
    public class AuctionsController : Controller
    {
        private readonly AuctionDbContext _context;
        private readonly IMapper _mapper;
        public AuctionsController(AuctionDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<AuctionDTO>>> GetAllAuctions()
        {
            var auctions = await _context.Auctions
                .Include(X => X.Item)
                .OrderBy(x => x.Item.Make)
                .ToListAsync();
            return _mapper.Map<List<AuctionDTO>>(auctions);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDTO>> GetAuctionById(Guid id)
        {
            var auction=await _context.Auctions.Include(x=>x.Item).FirstOrDefaultAsync(x=>x.Id == id);
            if (auction == null)
            {
                return NotFound();

            }
            return _mapper.Map<AuctionDTO>(auction);
        }
        [HttpPost]
        public async Task<ActionResult<AuctionDTO>> CreateAuction(CreateAuctionDto auctionDto)
        {
            var auction = _mapper.Map<Auction>(auctionDto);
            auction.Seller = "Test";
            _context.Auctions.Add(auction);
            var result = await _context.SaveChangesAsync() > 0;
            if (!result) return BadRequest("Could not save changes to the DB");
            return CreatedAtAction(nameof(GetAuctionById), new { auction.Id },_mapper.Map<AuctionDTO>(auction));
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
        {
            var auction =await _context.Auctions.Include(x=>x.Item).FirstOrDefaultAsync(x=>x.Id==id);
            if (auction == null) { return NotFound(); }
            auction.Item.Make = updateAuctionDto.Make??auction.Item.Make;
            auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
            auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
            auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
            auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

            var resut = await _context.SaveChangesAsync() > 0;
            if (resut) { return Ok(); }
            return BadRequest("Problem saving changes.");

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuction(Guid id) {
            var auction = await _context.Auctions.FindAsync(id);
            if (auction == null) { return NotFound(); }
            _context.Auctions.Remove(auction);
            var reuult = await _context.SaveChangesAsync()>0;
            if (!reuult) { return BadRequest("Could not update database"); }
            return Ok();
        }


    }
}
