﻿using FormsCreator.Application.Attributes;
using FormsCreator.Controllers.Base;
using FormsCreator.Core.DTOs.User;
using FormsCreator.Core.Interfaces.Services;
using FormsCreator.Core.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormsCreator.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("admin/manage-users")]
    public class UserController(IUserService userService) : AbsController
    {
        private readonly IUserService _userService = userService;

        [HttpGet]
        [ViewLayout("_AdminLayout")]
        public async Task<IActionResult> IndexAsync(CancellationToken token,
            bool getAll = true, bool getBlocked = false, int page = 1, int size = 10)
        {
            var usersRes = await GetUsersAsync(page, size, getAll, getBlocked, token);
            if (usersRes.IsFailure) return CustomViewResponse(usersRes);
            var viewRes = await ConfigureIndexAsync(page, size, getAll, getBlocked, token);
            if (viewRes.IsFailure) return CustomViewResponse(viewRes);
            return View(usersRes.Result);
        }

        [HttpGet("~/api/v1/users/search-by-term"), AllowAnonymous]
        public async Task<IActionResult> SearchByTermAsync(string q, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(q)) return Ok(Array.Empty<int>());
            var res = await _userService.SearchBySimilarityAsync(q, token);
            if (res.IsFailure) return CustomResponse(res);
            return Ok(res.Result);
        }

        [HttpPut("change-status/{userId:guid}")]
        public async Task<IActionResult> ChangeStatusAsync(Guid userId, bool newStatus)
        {
            var res = await _userService.ChangeStatusAsync(userId, newStatus);
            if (res.IsFailure) return CustomResponse(res);
            return Ok();
        }

        [HttpPut("change-role/{userId:guid}/{roleId:guid}")]
        public async Task<IActionResult> ChangeRoleAsync(Guid userId, Guid roleId)
        {
            var res = await _userService.ChangeRoleAsync(userId, roleId);
            if (res.IsFailure) return CustomResponse(res);
            return Ok();
        }

        [HttpDelete("delete/{userId:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid userId)
        {
            var res = await _userService.DeleteAsync(userId);
            if (res.IsFailure) return CustomResponse(res);
            return Ok();
        }

        [NonAction]
        Task<IResult<IEnumerable<UserPublicResponseDto>>> GetUsersAsync(int page, int size, bool getAll, bool getBlocked,
            CancellationToken token)
            => (getAll, getBlocked) switch
            {
                (true, false) => _userService.GetAllAsync(page, size, token),
                (false, false) => _userService.GetUnblockedAsync(page, size, token),
                (false, true) => _userService.GetBlockedAsync(page, size, token),
                _ => _userService.GetAllAsync(page, size, token)
            };

        [NonAction]
        async Task<Core.Shared.IResult> ConfigureIndexAsync(int page, int size, bool getAll, bool getBlocked, CancellationToken token)
        {
            ViewData["CurrentPage"] = page;
            ViewData["CurrentSize"] = size;
            ViewData["GetAll"] = getAll;
            ViewData["GetBlocked"] = getBlocked;
            var countRes = await CountAsync(getAll, getBlocked, token);
            if (countRes.IsFailure) return countRes;
            ViewData["UsersTotalCount"] = countRes.Result;
            return Result.Success();
        }

        [NonAction]
        Task<IResult<long>> CountAsync(bool getAll, bool getBlocked, CancellationToken token)
            => (getAll, getBlocked) switch
            {
                (true, false) => _userService.CountAllAsync(token),
                (false, false) => _userService.CountUnblockedAsync(token),
                (false, true) => _userService.CountBlockedAsync(token),
                _ => _userService.CountAllAsync(token)
            };
    }
}